using FluentValidation;
using To_Do.Interfaces.Common.Requests;

namespace To_Do.Services.Validation.RequestValidation;

public class CategoryValidation : AbstractValidator<CategoryRequest>
{
    public CategoryValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(48).WithMessage("Name cannot exceed 48 characters");;
    }
}