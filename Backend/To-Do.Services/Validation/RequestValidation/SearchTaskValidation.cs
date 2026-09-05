using FluentValidation;
using To_Do.Interfaces.Common.Requests;

namespace To_Do.Services.Validation.RequestValidation;

public class SearchTaskValidation : AbstractValidator<SearchTaskRequest>
{
    public SearchTaskValidation()
    {
        RuleFor(s => s.PageNumber)
            .InclusiveBetween(1, 100)
            .WithMessage("PageNumber must be between 1 and 100.");

        RuleFor(s => s.PageSize)
            .InclusiveBetween(1, 10)
            .WithMessage("PageSize must be between 1 and 10.");
    }
}