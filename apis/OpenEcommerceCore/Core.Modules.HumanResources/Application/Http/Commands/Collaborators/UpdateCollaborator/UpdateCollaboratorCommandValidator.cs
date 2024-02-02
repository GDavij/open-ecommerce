using Core.Modules.HumanResources.Domain.Contracts.Http.Commands.Collaborators.UpdateCollaborator;
using FluentValidation;

namespace Core.Modules.HumanResources.Application.Http.Commands.Collaborators.UpdateCollaborator;

internal class UpdateCollaboratorCommandValidator : AbstractValidator<UpdateCollaboratorCommand>
{
    public UpdateCollaboratorCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .NotEmpty().WithMessage("First Name must not be empty")
            .MaximumLength(255).WithMessage("First Name must have a maximum of 255 characters");
        
        RuleFor(c => c.LastName)
            .MaximumLength(255).When(c => !string.IsNullOrEmpty(c.LastName)).WithMessage("Last Name must have a maximum of 255 characters when not null or empty");
        
        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("Description must not be empty")
            .MaximumLength(2048).WithMessage("Description must have a maximum of 2048 characters");
        
        RuleFor(c => c.Age)
            .NotEmpty().WithMessage("Age must not be empty")
            .GreaterThan(12).WithMessage("Age must not be greater 12");
        
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("Email must not be empty")
            .EmailAddress().WithMessage("Email must be a valid one")
            .MaximumLength(255).WithMessage("Email must have a maximum of 255 characters");
        
        RuleFor(c => c.Phone)
            .NotEmpty().WithMessage("Phone must not be empty")
            .MaximumLength(22).WithMessage("Phone must have a maximum of 22 characters");
        
        RuleFor(c => c.Password)
            .Must(pw => pw != String.Empty).WithMessage("Password must not be empty")
            .MaximumLength(512).WithMessage("Password must not be greater than 512 characters");
        
        RuleForEach(c => c.Addresses).ChildRules(a =>
        {
            a.RuleFor(a => a.StateId)
                .NotEmpty().WithMessage("Address State Id must not be empty");
        
            a.RuleFor(a => a.ZipCode)
                .NotEmpty().WithMessage("Address Zip Code must not be empty")
                .GreaterThan(0).WithMessage("Address Zip Code must be greater than 0");
        
            a.RuleFor(a => a.Neighbourhood)
                .NotEmpty().WithMessage("Address Neighbourhood must not be empty")
                .MaximumLength(255).WithMessage("Address Neighbourhood must not be greater than 128 characters");
        
            a.RuleFor(a => a.Street)
                .NotEmpty().WithMessage("Address Street must not be empty")
                .MaximumLength(128).WithMessage("Address Street must not be greater than 128 characters");
        });
        
        RuleForEach(c => c.SocialLinks).ChildRules(s =>
        {
            s.RuleFor(s => s.SocialMedia)
                .IsInEnum().WithMessage("Social media must not be a invalid one");
        
            s.RuleFor(s => s.Url)
                .NotEmpty().WithMessage("Social Links Url must not be empty")
                .MaximumLength(384).WithMessage("Social Links Url must have a maximum of 384 characters");
        });
    }
}