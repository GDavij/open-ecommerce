using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries;

internal interface IGetCollaboratorQueryHandler
    : IRequestHandler<GetCollaboratorQuery, GetCollaboratorQueryResponse> 
{ }