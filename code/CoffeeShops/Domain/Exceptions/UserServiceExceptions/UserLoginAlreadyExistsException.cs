namespace CoffeeShops.Domain.Exceptions.UserServiceExceptions
{
    public class UserLoginAlreadyExistsException : Exception
    {
        public UserLoginAlreadyExistsException() { }

        public UserLoginAlreadyExistsException(string? message)
        : base(message) { }

        public UserLoginAlreadyExistsException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
}
