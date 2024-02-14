using Core.Modules.HumanResources.Domain.Enums;
using MediatR;

namespace Core.Modules.HumanResources.Domain.Contracts.Http.Commands.JobApplications.UpdateJobApplicationStatus;

public record UpdateJobApplicationStatusCommand(Guid Id, ApplicationProcess ProcessStatus) : IRequest<UpdateJobApplicationStatusCommandResponse>; 

public record HttpApiUpdateJobApplicationStatusRequestSchema(ApplicationProcess Status);