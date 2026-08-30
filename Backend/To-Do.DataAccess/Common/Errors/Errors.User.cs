using ErrorOr;

namespace To_Do.DataAccess.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail(string email) => Error.Conflict(
            code: "User.DuplicateEmail",
            description: $"The email '{email}' is already in use.");
        
        public static Error DuplicateLogin(string login) => Error.Conflict(
            code: "User.DuplicateLogin",
            description: $"The login '{login}' is already in use.");
    }
}