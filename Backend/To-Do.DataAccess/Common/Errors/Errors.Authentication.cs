using ErrorOr;

namespace To_Do.DataAccess.Common.Errors;

public partial class Errors
{
    public static class Authentication
    {
        public static Error InvalidCredentials => Error.Validation(
            code: "Auth.InvalidCredential",
            description: "Invalid credentials.");
    }
}