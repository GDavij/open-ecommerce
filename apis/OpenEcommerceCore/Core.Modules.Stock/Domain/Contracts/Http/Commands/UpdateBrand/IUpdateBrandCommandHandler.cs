using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateBrand;

public interface IUpdateBrandCommandHandler
    : IRequestHandler<UpdateBrandCommand, UpdateBrandCommandResponse>
{ }