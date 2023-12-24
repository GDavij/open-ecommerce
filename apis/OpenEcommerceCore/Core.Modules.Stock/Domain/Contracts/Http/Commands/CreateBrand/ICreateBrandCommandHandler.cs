using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateBrand;

internal interface ICreateBrandCommandHandler 
    : IRequestHandler<CreateBrandCommand, CreateBrandCommandResponse> 
{ }