using System.Data;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.CreateProduct;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.CreateProduct;

// Create Validator for Command
internal class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(c => c.BrandId)
            .NotEmpty().WithMessage("Brand Id must not be empty");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name must not be empty")
            .MaximumLength(255).WithMessage("Name must not have more than 255 characters");

        RuleFor(c => c.Description)
            .MaximumLength(512).When(d => d.Description is not null).WithMessage("Description must have a maximum of 512 characters");

        RuleFor(c => c.Sku)
            .MaximumLength(20).When(c => c.Sku is not null).WithMessage("SKU must have a maximum of 20 characters")
            .MinimumLength(4).When(c => c.Sku is not null).WithMessage("Sku must have a minimum of 4 characters when not empty");

        RuleFor(c => c.Ean)
            .NotEmpty().WithMessage("EAN-13 must not be empty")
            .Length(13).WithMessage("EAN-13 Code must have 13 digits");

        RuleFor(c => c.Upc)
            .Length(12).When(c => c.Upc is not null);

        RuleFor(c => c.Price)
            .NotEmpty().WithMessage("Price must not be empty")
            .PrecisionScale(16,2, false).WithMessage("Precision of Price must not be higher than 16 and scale not higher than 2 - (Trailing zeros are considered in precision)");

        RuleFor(c => c.StockUnitCount)
            .NotEmpty().WithMessage("Stock Unit Count must not be empty");

        RuleFor(c => c.TagsIds)
            .NotNull().WithMessage("Tags Ids must not be null");

        RuleForEach(c => c.TagsIds).ChildRules(tag
            => tag.RuleFor(t => t)
                .NotEmpty().WithMessage("Product Tag must not be empty"));

        RuleFor(c => c.Measurements)
            .NotNull().WithMessage("Measurements must not be null");

        RuleForEach(c => c.Measurements).ChildRules(measure =>
        {
            measure.RuleFor(m => m.Name)
                .NotEmpty().WithMessage("Measure Name must not be empty")
                .MaximumLength(255).WithMessage("Measure Name must have a maximum of 255 characters");

            measure.RuleFor(m => m.Value)
                .NotEmpty().WithMessage("Measure Value must not be empty")
                .MaximumLength(255).WithMessage("Measure Value must have a maximum of 255 characters");

            measure.RuleFor(m => m.ShowOrder)
                .NotEmpty().WithMessage("Measure Show Order must not be empty")
                .GreaterThan(0).WithMessage("Measure Show Order must be greater than 0");
            
            measure.RuleFor(m => m.MeasureUnitId)
        });
    }
}