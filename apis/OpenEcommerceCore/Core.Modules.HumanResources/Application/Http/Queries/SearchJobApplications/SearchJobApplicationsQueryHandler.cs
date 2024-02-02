using Core.Modules.HumanResources.Domain.Contracts.Context;
using Core.Modules.HumanResources.Domain.Contracts.Http.Queries.SearchJobApplication;
using Microsoft.EntityFrameworkCore;

namespace Core.Modules.HumanResources.Application.Http.Queries.SearchJobApplications;

internal class SearchJobApplicationsQueryHandler : ISearchJobApplicationsQueryHandler
{
    private readonly IHumanResourcesContext _dbContext;

    public SearchJobApplicationsQueryHandler(IHumanResourcesContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<SearchJobApplicationsQueryResponse>> Handle(SearchJobApplicationsQuery request, CancellationToken cancellationToken)
    {
        var applicationsQueryable = _dbContext.JobApplications
            .Include(j => j.SocialLinks)
            .Where(x => EF.Functions.ILike("%" + x.FirstName + "%" + x.LastName + "%" + x.Email + "%" + x.Phone + "%" + x.Age + "%", $"%{request.SearchTerm.Replace(' ', '%')}%"));

        var shouldFilterByProcessStep = request.ProcessStep != null;
        if (shouldFilterByProcessStep)
        {
            applicationsQueryable = applicationsQueryable.Where(x => x.ProcessStep == request.ProcessStep);
        }

        var shouldFilterBySector = request.Sector != null;
        if (shouldFilterBySector)
        {
            applicationsQueryable = applicationsQueryable.Where(x => x.Sector == request.Sector);
        }

        return await applicationsQueryable
            .Select(x => new SearchJobApplicationsQueryResponse
            {
                Id = x.Id,
                FullName = $"{x.FirstName} {x.LastName}",
                Age = x.Age,
                Email = x.Email,
                Phone = x.Phone,
                ProcessStep = x.ProcessStep,
                Sector = x.Sector,
                SentAt = x.CreatedAt
            })
            .ToListAsync(cancellationToken);
    }
}