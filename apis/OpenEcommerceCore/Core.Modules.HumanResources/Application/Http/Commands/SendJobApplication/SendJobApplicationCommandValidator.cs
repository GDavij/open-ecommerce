using Core.Modules.HumanResources.Domain.Constants;
using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.SendJobApplication;
using Core.Modules.Shared.Domain.Constants;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Commands.SendJobApplication;

internal class SendJobApplicationCommandValidator : AbstractValidator<SendJobApplicationCommand>
{
    public SendJobApplicationCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage("First name must not be empty")
            .MaximumLength(255).WithMessage("First name must not be greater than 255 characters");

        RuleFor(c => c.LastName)
            .MaximumLength(255).When(c => c.LastName is not null).WithMessage("Last name must not be greater than 255 characters");

        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email must not be empty")
            .MaximumLength(255).WithMessage("Email must not be greater than 255 characters")
            .EmailAddress().WithMessage("Email must be a valid email address");
        
        RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Phone must not be empty")
            .MaximumLength(22).WithMessage("Phone must not be greater than 22 characters");

        RuleFor(c => c.Age)
            .NotEmpty().WithMessage("Age must not be empty")
            .GreaterThan(12).WithMessage("Age must be greater than 12");

        RuleFor(c => c.Sector)
            .IsInEnum().WithMessage("Sector must be valid");

        RuleFor(c => c.Resume).ChildRules(r =>
        {
            r.RuleFor(r => r.ContentType)
                .NotEmpty().WithMessage("MIMETYPE must not be empty")
                .Must(ct => AllowedMimeTypes.JobApplicationMimeTypes.Contains(ct.Split('/')[1]))
                .WithMessage("Resume must be of a valid MIMETYPE Format");

            var twoMegabytes = MemoryMeasure.Megabytes * 2;
            r.RuleFor(r => r.Length)
                .NotEmpty().WithMessage("Pdf Length must not be empty")
                .LessThanOrEqualTo(twoMegabytes).WithMessage("Pdf must not be greater than 2MB")
                .GreaterThan(0).WithMessage("Pdf must not be 0 bytes or any negative invalid byte");
        });

        RuleForEach(c => c.SocialLinks).ChildRules(s =>
        {
            s.RuleFor(s => s.Url)
                .NotEmpty().WithMessage("Social Link URL must not be empty")
                .MaximumLength(384).WithMessage("Social Link must not be greater than 384 characters");

            s.RuleFor(s => s.SocialMedia)
                .IsInEnum().WithMessage("Social Media must be valid");
        });
    }
}