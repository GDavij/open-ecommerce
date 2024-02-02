using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Collaborators.GetCollaborator;

internal interface IGetCollaboratorQueryHandler
    : IRequestHandler<GetCollaboratorQuery, GetCollaboratorQueryResponse> 
{ }