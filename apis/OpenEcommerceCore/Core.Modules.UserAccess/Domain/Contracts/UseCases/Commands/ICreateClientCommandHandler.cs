using Core.Modules.UserAccess.Application.UseCases.Commands.CreateClient;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;

public interface ICreateClientCommandHandler 
    : IConsumer<CreateClientCommand>
{ }