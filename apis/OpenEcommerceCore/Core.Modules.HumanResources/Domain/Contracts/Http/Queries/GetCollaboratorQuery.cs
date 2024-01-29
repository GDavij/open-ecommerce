using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries;

public record GetCollaboratorQuery(Guid Id): IRequest<GetCollaboratorQueryResponse>;