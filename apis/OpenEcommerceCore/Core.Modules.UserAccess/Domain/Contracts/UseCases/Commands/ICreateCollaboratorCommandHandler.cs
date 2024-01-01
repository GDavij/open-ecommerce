using Core.Modules.UserAccess.Application.UseCases.Commands.CreateCollaborator;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;

public interface ICreateCollaboratorCommandHandler
    : IConsumer<CreateCollaboratorCommand>
{ }