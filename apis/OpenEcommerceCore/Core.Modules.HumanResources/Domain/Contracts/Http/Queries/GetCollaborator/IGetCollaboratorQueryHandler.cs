using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.GetCollaborator;

internal interface IGetCollaboratorQueryHandler
    : IRequestHandler<GetCollaboratorQuery, GetCollaboratorQueryResponse> 
{ }