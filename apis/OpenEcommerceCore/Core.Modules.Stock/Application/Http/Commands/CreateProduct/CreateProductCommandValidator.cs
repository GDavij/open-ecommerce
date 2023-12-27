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
            .MinimumLength(1).WithMessage("Name must not have less than 1 character")
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
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater or equal to zero ")
            .PrecisionScale(16,2, false).WithMessage("Precision of Price must not be higher than 16 and scale not higher than 2 - (Trailing zeros are considered in precision)");

        RuleFor(c => c.StockUnitCount)
            .NotEmpty().WithMessage("Stock Unit Count must not be empty")
            .GreaterThanOrEqualTo(0).WithMessage("Stock Unit must be greater or equal to zero");

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
                .NotEmpty().When(m => m.MeasureUnitId is not null).WithMessage("Measure Unit Should not be empty");
        });
        
        RuleFor(c => c.TechnicalDetails)
            .NotNull().WithMessage("Technical Details must not be null");

        RuleForEach(c => c.TechnicalDetails).ChildRules(technicalDetail =>
        {
            technicalDetail.RuleFor(t => t.Name)
                .NotEmpty().WithMessage("Technical Detail Name must not be empty")
                .MaximumLength(255).WithMessage("Technical Detail Name must have a maximum of 255 characters");

            technicalDetail.RuleFor(t => t.Value)
                .NotEmpty().WithMessage("Technical Detail Value must not be empty")
                .MaximumLength(255).WithMessage("Technical Detail Value must have a maximum of 255 characters");

            technicalDetail.RuleFor(t => t.ShowOrder)
                .NotEmpty().WithMessage("Technical Detail Show Order must not be empty")
                .GreaterThan(0).WithMessage("Technical Detail Show Order must be greater than 0");

            technicalDetail.RuleFor(t => t.MeasureUnitId)
                .NotEmpty().When(t => t.MeasureUnitId is not null).WithMessage("Technical Detail Unit Should not be empty");
        });
        
        RuleFor(c => c.OtherDetails)
            .NotNull().WithMessage("Other Details must not be null");

        RuleForEach(c => c.OtherDetails).ChildRules(otherDetail =>
        {
            otherDetail.RuleFor(o => o.Name)
                .NotEmpty().WithMessage("Other Detail Name must not be empty")
                .MaximumLength(255).WithMessage("Other Detail Name must have a maximum of 255 characters");

            otherDetail.RuleFor(m => m.Value)
                .NotEmpty().WithMessage("Other Detail Value must not be empty")
                .MaximumLength(255).WithMessage("Other Detail Value must have a maximum of 255 characters");

            otherDetail.RuleFor(o => o.ShowOrder)
                .NotEmpty().WithMessage("Other Detail Show Order must not be empty")
                .GreaterThan(0).WithMessage("Other Detail Show Order must be greater than 0");
            
            otherDetail.RuleFor(o => o.MeasureUnitId)
                .NotEmpty().When(o => o.MeasureUnitId is not null).WithMessage("Other Detail Unit Should not be empty");
        });
    }
}