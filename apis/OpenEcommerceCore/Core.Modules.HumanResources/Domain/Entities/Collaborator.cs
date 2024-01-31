using Core.Modules.Shared.Domain.Helpers;
using Core.Modules.Shared.Domain.ResultObjects;
using Core.Modules.Shared.Messaging.Commands.UserAccess;
using Core.Modules.Shared.Messaging.Queries.UserAccess.Collaborators;
using MassTransit;

namespace Core.Modules.HumanResources.Domain.Entities;

internal sealed class Collaborator
{
    public Guid Id { get; init; }
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string Description { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public List<Contract> Contracts { get; init; }
    public List<SocialLink> SocialLinks { get; set; }
    public List<Address> Addresses { get; set; }
    public int TotalContributionYears => Contracts.Sum(c => c.ContributionYears.Count);
    public double TotalHoursWorked => Contracts.Sum(c => c.ContributionYears.Sum(cy => cy.WorkHours.Sum(wh => wh.Duration.Ticks)));
    
    public async Task<bool> IsAdmin(IRequestClient<GetCollaboratorIsAdminQuery> requestClient)
    {
        var result = await requestClient.GetResponse<EvaluationResult<ValueTypeWrapper<bool>>>(new GetCollaboratorIsAdminQuery { Id = Id });
        return result.Message.Eval.Value;
    }

    public async Task<bool> IsDeleted(IRequestClient<GetCollaboratorIsDeletedQuery> requestClient)
    {
        var result = await requestClient.GetResponse<EvaluationResult<ValueTypeWrapper<bool>>>(new GetCollaboratorIsDeletedQuery(Id));
        return result.Message.Eval.Value;
    }
}