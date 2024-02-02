using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Collaborators.GetCollaborator;

public record GetCollaboratorQuery(Guid Id): IRequest<GetCollaboratorQueryResponse>;