namespace CoffeeShops.Domain.Exceptions.UserServiceExceptions
{
    public class UserIncorrectAtributeException : Exception
    {
        public UserIncorrectAtributeException() { }

        public UserIncorrectAtributeException(string? message)
        : base(message) { }

        public UserIncorrectAtributeException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
}
