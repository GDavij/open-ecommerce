using Core.Modules.HumanResources.Domain.IntegrationEvents.Events.Collaborators;
using MassTransit;

namespace Core.Modules.UserAccess.Domain.Contracts.UseCases.Commands;

public interface ICreateCollaboratorCommandHandler
    : IConsumer<CreatedCollaboratorIntegrationEvent>
{ }