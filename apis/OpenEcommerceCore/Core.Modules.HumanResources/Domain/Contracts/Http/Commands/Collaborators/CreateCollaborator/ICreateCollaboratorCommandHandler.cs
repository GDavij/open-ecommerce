using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Collaborators.CreateCollaborator;

internal interface ICreateCollaboratorCommandHandler 
    : IRequestHandler<CreateCollaboratorCommand, CreateCollaboratorCommandResponse>
{ }