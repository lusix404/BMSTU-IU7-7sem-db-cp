namespace CoffeeShops.Domain.Exceptions.CategoryServiceExceptions
{
    public class CategoryUniqueException : Exception
    {
        public CategoryUniqueException() { }

        public CategoryUniqueException(string? message)
        : base(message) { }

        public CategoryUniqueException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
