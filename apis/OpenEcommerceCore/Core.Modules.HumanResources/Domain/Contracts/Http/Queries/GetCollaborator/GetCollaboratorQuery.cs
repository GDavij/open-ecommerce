using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.GetCollaborator;

public record GetCollaboratorQuery(Guid Id): IRequest<GetCollaboratorQueryResponse>;