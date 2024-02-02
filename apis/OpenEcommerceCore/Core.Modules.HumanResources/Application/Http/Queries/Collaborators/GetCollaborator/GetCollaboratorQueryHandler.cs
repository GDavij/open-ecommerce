using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Collaborators.GetCollaborator;
using Core.Modules.HumanResources.Domain.Exceptions.Collaborators;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Administrators;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Collaborators;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Queries.Collaborators.GetCollaborator;

internal class GetCollaboratorQueryHandler : IGetCollaboratorQueryHandler
{
    private readonly IHumanResourcesContext _dbContext;
    private readonly IRequestClient<GetCollaboratorIsAdminQuery> _getCollaboratorIsAdminClient;
    private readonly IRequestClient<GetCollaboratorIsDeletedQuery> _getCollaboratorIsDeletedClient;
    private readonly IRequestClient<GetAdministratorsIdsQuery> _getAdministratorsIdsClient;
    
    public GetCollaboratorQueryHandler(
        IHumanResourcesContext dbContext,
        IRequestClient<GetCollaboratorIsDeletedQuery> getCollaboratorIsDeletedClient,
        IRequestClient<GetCollaboratorIsAdminQuery> getCollaboratorIsAdminClient,
        IRequestClient<GetAdministratorsIdsQuery> getAdministratorsIdsClient)
    {
        _dbContext = dbContext;
        _getCollaboratorIsDeletedClient = getCollaboratorIsDeletedClient;
        _getCollaboratorIsAdminClient = getCollaboratorIsAdminClient;
        _getAdministratorsIdsClient = getAdministratorsIdsClient;
    }

    public async Task<GetCollaboratorQueryResponse> Handle(GetCollaboratorQuery request, CancellationToken cancellationToken)
    {
        var administratorsIds = (await _getAdministratorsIdsClient.GetResponse<EvaluationResult<HashSet<Guid>>>(new GetAdministratorsIdsQuery())).Message.Eval; 
        var existentCollaborator = await _dbContext.Collaborators
            .Include(c => c.Contracts)
                .ThenInclude(c => c.ContributionYears)
                    .ThenInclude(c => c.WorkHours)
            .Include(c => c.Addresses)
                .ThenInclude(c => c.State)
            .Include(c => c.SocialLinks)
            .FirstOrDefaultAsync(c => c.Id == request.Id && !administratorsIds.Contains(c.Id), cancellationToken);

        if (existentCollaborator is null)
        {
            throw new InvalidCollaboratorException(request.Id);
        }
        
        var response = new GetCollaboratorQueryResponse
        {
            Id = existentCollaborator.Id,
            FirstName = existentCollaborator.FirstName,
            LastName = existentCollaborator.LastName,
            Description = existentCollaborator.Description,
            Age = existentCollaborator.Age,
            Email = existentCollaborator.Email,
            Phone = existentCollaborator.Phone,
            TotalContributionYears = existentCollaborator.TotalContributionYears,
            TotalHoursWorked = existentCollaborator.TotalHoursWorked,
            IsAdmin = await existentCollaborator.IsAdmin(_getCollaboratorIsAdminClient),
            IsDeleted = await existentCollaborator.IsDeleted(_getCollaboratorIsDeletedClient),
            Contracts = existentCollaborator.Contracts.Select(c => new GetCollaboratorQueryResponse.ContractResponse
            {
                Id = c.Id,
                CollaboratorId = c.CollaboratorId,
                Name = c.Name,
                CollaboratorSector = c.Sector,
                StartDate = c.StartDate,
                EndDate = c.EndDate,
                IsInForce = !c.Broken && DateTime.UtcNow <= c.EndDate,
                MonthlySalary = c.MonthlySalary,
                Expired = c.Expired,
                Broken = c.Broken,
                Deleted = c.Deleted
            }).ToList(),
            Addresses = existentCollaborator.Addresses.Select(a => new GetCollaboratorQueryResponse.AddressResponse
            {
                Id = a.Id,
                CollaboratorId = a.CollaboratorId,
                State = new GetCollaboratorQueryResponse.AddressResponse.StateResponse
                {
                    Id = a.State.Id,
                    Name = a.State.Name,
                    ShortName = a.State.ShortName
                },
                ZipCode = a.ZipCode,
                Neighbourhood = a.Neighbourhood,
                Street = a.Street
            }).ToList(),
            SocialLinks = existentCollaborator.SocialLinks.Select(s => new GetCollaboratorQueryResponse.SocialLinkResponse
            {
                Id = s.Id,
                CollaboratorId = (Guid)s.CollaboratorId!,
                SocialMedia = s.SocialMedia,
                URL = s.URL
            }).ToList()
        };

        return response; 
    }
}