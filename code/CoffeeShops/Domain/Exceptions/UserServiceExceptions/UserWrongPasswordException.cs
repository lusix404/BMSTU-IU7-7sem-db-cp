namespace CoffeeShops.Domain.Exceptions.UserServiceExceptions
{
    public class UserWrongPasswordException : Exception
    {
        public UserWrongPasswordException() { }

        public UserWrongPasswordException(string? message)
        : base(message) { }

        public UserWrongPasswordException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
}
