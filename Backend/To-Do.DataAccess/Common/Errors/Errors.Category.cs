using ErrorOr;

namespace To_Do.DataAccess.Common.Errors;

public partial class Errors
{
    public static class Category
    {
        public static Error NotFound(Guid categoryId) => Error.NotFound(
            code: "Category.NotFound",
            description: "This category do not exists.");
        
        public static Error AlreadyExists(string categoryName) => Error.Conflict(
            code: "Category.AlreadyExists",
            description: $"Category {categoryName} already exists.");
    }
}