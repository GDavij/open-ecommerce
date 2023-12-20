using Core.Modules.Shared.Domain.Constants;
using Core.Modules.Stock.Domain.Constants;
using Core.Modules.Stock.Domain.Contracts.Http.Commands.AddImageToProduct;
using FluentValidation;

namespace Core.Modules.Stock.Application.Http.Commands.AddImageToProduct;

internal class AddImageToProductCommandValidator 
    : AbstractValidator<AddImageToProductCommand>
{
    public AddImageToProductCommandValidator()
    {
        RuleFor(c => c.ProductId)
            .NotEmpty().WithMessage("Product Id must not be empty");

        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("Image Description must not be empty")
            .MinimumLength(8).WithMessage("Image Description must have at least 8 characters")
            .MaximumLength(255).WithMessage("Image Description must not have more than 255 characters");

        RuleFor(c => c.ImageFile)
            .NotEmpty().WithMessage("Image File must not be empty");

        RuleFor(c => c.ImageFile.ContentType)
            .NotEmpty().WithMessage("Image Content Type must not be empty")
            .Must(ct => AllowedMimeContentTypes.MimeTypes.Contains(ct)).WithMessage("Invalid image MIMETYPE was found");

        var twoMegabytes = MemoryMeasure.Megabytes * 2;
        RuleFor(c => c.ImageFile.Length)
            .NotEmpty().WithMessage("Image Length must not be empty")
            .LessThanOrEqualTo(twoMegabytes).WithMessage($"Image largest than {twoMegabytes} bytes, image file size outside of limit");
    }
}