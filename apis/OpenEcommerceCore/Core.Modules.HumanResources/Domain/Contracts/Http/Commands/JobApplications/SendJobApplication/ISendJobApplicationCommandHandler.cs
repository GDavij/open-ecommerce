using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.JobApplications.SendJobApplication;

internal interface ISendJobApplicationCommandHandler 
    : IRequestHandler<SendJobApplicationCommand>
{ }