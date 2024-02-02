using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SendJobApplication;

internal interface ISendJobApplicationCommandHandler 
    : IRequestHandler<SendJobApplicationCommand>
{ }