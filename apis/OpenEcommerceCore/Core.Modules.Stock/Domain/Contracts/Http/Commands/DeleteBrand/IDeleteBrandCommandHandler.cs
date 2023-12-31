using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.DeleteBrand;

internal interface IDeleteBrandCommandHandler
    : IRequestHandler<DeleteBrandCommand>
{ }