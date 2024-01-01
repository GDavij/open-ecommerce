using Core.Modules.Stock.Domain.Contracts.Http.Commands.UpdateProduct;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.UpdateProduct;

internal class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty().WithMessage("Product Id must not be empty");

        RuleFor(c => c.BrandId)
            .NotEmpty().WithMessage("Brand Id must not be empty");

        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("Name must not be empty")
            .MaximumLength(255).WithMessage("Name must have a max of 255 characters");

        RuleFor(c => c.Description)
            .MaximumLength(512).When(c => !string.IsNullOrEmpty(c.Description)).WithMessage("Description must have a maximum of 512 characters");

        RuleFor(c => c.Sku)
            .MaximumLength(20).When(c => !string.IsNullOrEmpty(c.Sku)).WithMessage("SKU must have a maximum of 20 characters when not empty")
            .MinimumLength(4).When(c => !string.IsNullOrEmpty(c.Sku)).WithMessage("SKU Must have a minimum of 4 characters when not empty");

        RuleFor(c => c.Ean)
            .NotEmpty().WithMessage("EAN-13 must not be empty")
            .Length(13).WithMessage("EAN-13 Code must have 13 digits");

        RuleFor(c => c.Upc)
            .Length(12).When(c => !string.IsNullOrEmpty(c.Upc)).WithMessage("UPC-A must have a length of 12 when not empty");

        RuleFor(c => c.Price)
            .NotEmpty().WithMessage("Price must not be empty")
            .GreaterThan(0).WithMessage("Price must be greater than zero")
            .PrecisionScale(16, 2, false).WithMessage("Precision of Price must not be higher than 16 and scale not higher than 2 - (Trailing zeros are considered in precision)");

        RuleFor(c => c.StockUnitCount)
            .GreaterThanOrEqualTo(0).WithMessage("Stock Unit Count must be greater than or equal to 0");

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