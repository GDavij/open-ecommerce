using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.Collaborators.SearchCollaborators;

internal interface ISearchCollaboratorsQueryHandler
    : IRequestHandler<SearchCollaboratorsQuery, List<SearchCollaboratorsQueryResponse>>
{ }