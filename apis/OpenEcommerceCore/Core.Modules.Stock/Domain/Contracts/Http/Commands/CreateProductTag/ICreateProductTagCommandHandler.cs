using MediatR;

namespace Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProductTag;

public interface ICreateProductTagCommandHandler
    : IRequestHandler<CreateProductTagCommand, CreateProductTagCommandResponse>
{ }