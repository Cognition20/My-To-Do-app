using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace To_Do.Services.Validation;

public class ValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
                continue;

            var validatorType = typeof(IValidator<>)
                .MakeGenericType(argument.GetType());

            var validator = serviceProvider.GetService(validatorType);

            if (validator is null)
                continue;

            var validationContextType = typeof(ValidationContext<>)
                .MakeGenericType(argument.GetType());

            var validationContext = Activator.CreateInstance(
                validationContextType,
                argument);

            var result = await ((IValidator)validator).ValidateAsync(
                (IValidationContext)validationContext!,
                context.HttpContext.RequestAborted);

            if (!result.IsValid)
            {
                var modelState = new ModelStateDictionary();

                foreach (var error in result.Errors)
                {
                    modelState.AddModelError(
                        error.PropertyName,
                        error.ErrorMessage);
                }

                context.Result = new BadRequestObjectResult(
                    new ValidationProblemDetails(modelState));

                return;
            }
        }

        await next();
    }
}