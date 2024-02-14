using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.JobApplications.UpdateJobApplicationStatus;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Commands.JobApplications.UpdateJobApplicationStatus;

internal class UpdateJobApplicationStatusCommandValidator :  AbstractValidator<UpdateJobApplicationStatusCommand>
{
    public UpdateJobApplicationStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Id must not be empty");

        RuleFor(x => x.ProcessStatus)
            .IsInEnum().WithMessage("Process Status must be in the Enum");
    }
}