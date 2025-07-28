namespace CoffeeShops.Domain.Exceptions.CategoryServiceExceptions
{
    public class CategoryIncorrectAtributeException : Exception
    {
        public CategoryIncorrectAtributeException() { }

        public CategoryIncorrectAtributeException(string? message)
        : base(message) { }

        public CategoryIncorrectAtributeException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
