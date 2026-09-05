using FluentValidation;
using To_Do.Interfaces.Common.Requests;

namespace To_Do.Services.Validation.RequestValidation;

public class RegisterValidation : AbstractValidator<RegisterRequest>
{
    public RegisterValidation()
    {
        RuleFor(r => r.Login)
            .NotEmpty().WithMessage("Login is required.")
            .MinimumLength(3).WithMessage("Login must be at least 3 characters long.")
            .MaximumLength(36).WithMessage("Login must be less than 36 characters long.");
        
        RuleFor(r => r.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email is invalid.")
            .MaximumLength(96).WithMessage("Email must be less than 96 characters long.");
        
        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(28).WithMessage("Password must be less than 28 characters long.");
    }
}