using ErrorOr;

namespace To_Do.DataAccess.Common.Errors;

public partial class Errors
{
    public static class ToDo
    {
        public static Error NotFound() => Error.Conflict(
            code: "Task.NotFound",
            description: "This task do not exists.");
    }
}