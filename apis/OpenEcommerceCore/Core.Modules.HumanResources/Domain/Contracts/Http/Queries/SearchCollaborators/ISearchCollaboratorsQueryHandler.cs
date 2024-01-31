using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Queries.SearchCollaborators;

internal interface ISearchCollaboratorsQueryHandler
    : IRequestHandler<SearchCollaboratorsQuery, List<SearchCollaboratorsQueryResponse>>
{ }