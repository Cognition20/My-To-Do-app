using FluentValidation;
using To_Do.Interfaces.Common.Requests;

namespace To_Do.Services.Validation.RequestValidation;

public class LoginValidation : AbstractValidator<LoginRequest>
{
    public LoginValidation()
    {
        RuleFor(l => l.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is invalid.")
            .MaximumLength(96).WithMessage("Email must be less than 96 characters long.");
        
        RuleFor(l => l.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(28).WithMessage("Password must be less than 28 characters long.");
    }
}