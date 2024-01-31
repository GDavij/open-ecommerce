using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.SearchCollaborators;
using Core.Modules.HumanResources.Domain.Extensions;
using Core.Modules.Shared.Domain.BusinessHierarchy;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Administrators;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Collaborators;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Queries.SearchCollaborators;

internal class SearchCollaboratorsQueryHandler : ISearchCollaboratorsQueryHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly IRequestClient<GetAdministratorsIdsQuery> _getAdministratorsIdsClient;
    private readonly IRequestClient<GetDeletedCollaboratorsIdsCommandQuery> _getDeletedCollaboratorsIdsClient;
    private readonly IRequestClient<GetCollaboratorsIdsFromSectorQuery> _getCollaboratorsIdsFromSectorClient;
    
    public SearchCollaboratorsQueryHandler(IHumanResourcesContext dbContext, IRequestClient<GetAdministratorsIdsQuery> getAdministratorsIdsClient, IRequestClient<GetDeletedCollaboratorsIdsCommandQuery> getDeletedCollaboratorsIdsClient, IRequestClient<GetCollaboratorsIdsFromSectorQuery> getCollaboratorsIdsFromSectorClient)
    {
        _dbContext = dbContext;
        _getAdministratorsIdsClient = getAdministratorsIdsClient;
        _getDeletedCollaboratorsIdsClient = getDeletedCollaboratorsIdsClient;
        _getCollaboratorsIdsFromSectorClient = getCollaboratorsIdsFromSectorClient;
    }

    public async Task<List<SearchCollaboratorsQueryResponse>> Handle(SearchCollaboratorsQuery request, CancellationToken cancellationToken)
    {
        bool shouldFilterIsDeleted = request.IsDeleted != null;
        bool shouldFilterBySector = request.Sector != null;
        
        
        var collaboratorsQueryable = _dbContext.Collaborators
            .Include(c => c.Contracts)
            .Include(c => c.Addresses)
                .ThenInclude(c => c.State)
            .AsQueryable();
        
        
        //TODO: optimize Query complex costly searches
        collaboratorsQueryable = collaboratorsQueryable.Where(c =>  
            (EF.Functions.ILike("%" + c.FirstName + "%" + c.LastName + "%" + c.Description + "%" + c.Email + "%" + c.Age + "%" + c.Phone + "%", $"%{request.SearchTerm.Replace(' ', '%')}%") ||
            c.Addresses.Any(a => EF.Functions.ILike(a.Neighbourhood + "%" + a.Street + "%" + a.ZipCode + "%" + a.State.Name + "%" + a.State.ShortName + "%", $"%{request.SearchTerm.Replace(' ', '%')}%"))) || 
            c.Contracts.Any(cont => EF.Functions.ILike("%" + cont.Name + "%", $"%{request.SearchTerm.Replace(' ', '%')}%")));
        
        var deletedCollaboratorsIds = (await _getDeletedCollaboratorsIdsClient.GetResponse<EvaluationResult<HashSet<Guid>>>(new GetDeletedCollaboratorsIdsCommandQuery(), cancellationToken)).Message.Eval;
        if (shouldFilterIsDeleted)
        {
            collaboratorsQueryable = request.IsDeleted == true 
                ? collaboratorsQueryable.Where(c => deletedCollaboratorsIds.Contains(c.Id)) 
                : collaboratorsQueryable.Where(c => !deletedCollaboratorsIds.Contains(c.Id));
        }

        if (shouldFilterBySector)
        {
            var collaboratorsFromSector = (await _getCollaboratorsIdsFromSectorClient.GetResponse<EvaluationResult<HashSet<Guid>>>(new GetCollaboratorsIdsFromSectorQuery((ECollaboratorSector)request.Sector!))).Message.Eval;
            collaboratorsQueryable = collaboratorsQueryable.Where(c => collaboratorsFromSector.Contains(c.Id));
        }
        
        var administratorsIds = (await _getAdministratorsIdsClient.GetResponse<EvaluationResult<HashSet<Guid>>>(new GetAdministratorsIdsQuery(), cancellationToken)).Message.Eval;
        
        var collaborators = await collaboratorsQueryable
            .Where(c => !administratorsIds.Contains(c.Id))
            .ToListAsync(cancellationToken);
        
        return collaborators.Select(c => new SearchCollaboratorsQueryResponse 
        { 
            Id = c.Id, 
            FullName = $"{c.FirstName} {c.LastName}", 
            Description = c.Description, 
            Email = c.Email, 
            Age = c.Age, 
            Phone = c.Phone, 
            Deleted = GetIsDeletedOptimized(c.Id, deletedCollaboratorsIds, request.IsDeleted) 
        }).ToList();
    }

    private static bool GetIsDeletedOptimized(Guid id, HashSet<Guid> deletedCollaboratorsIds, bool? searchIsDeleted)
    {
        if (searchIsDeleted != null)
        {
            return searchIsDeleted == true;
        }

        return deletedCollaboratorsIds.Contains(id);
    }
}