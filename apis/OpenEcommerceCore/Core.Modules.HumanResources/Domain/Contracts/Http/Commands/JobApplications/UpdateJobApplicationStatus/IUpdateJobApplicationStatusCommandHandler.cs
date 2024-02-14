using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.JobApplications.UpdateJobApplicationStatus;

internal interface IUpdateJobApplicationStatusCommandHandler
    : IRequestHandler<UpdateJobApplicationStatusCommand, UpdateJobApplicationStatusCommandResponse>
{ }