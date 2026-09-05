using FluentValidation;
using To_Do.Interfaces.Common.Requests;

namespace To_Do.Services.Validation.RequestValidation;

public class UpdateTaskValidation : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskValidation()
    {
        RuleFor(request => request.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(48).WithMessage("Title cannot exceed 48 characters.");
        
        RuleFor(request => request.Description)
            .MaximumLength(200).WithMessage("Description is too long.");
    }
}
