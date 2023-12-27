using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateBrand;

public interface ICreateBrandCommandHandler 
    : IRequestHandler<CreateBrandCommand, CreateBrandCommandResponse> 
{ }