using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.CreateCollaborator;

internal interface ICreateCollaboratorCommandHandler 
    : IRequestHandler<CreateCollaboratorCommand, CreateCollaboratorCommandResponse>
{ }