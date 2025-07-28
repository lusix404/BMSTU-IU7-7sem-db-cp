namespace CoffeeShops.Domain.Exceptions.UserServiceExceptions
{
    public class UserLoginNotFoundException : Exception
    {
        public UserLoginNotFoundException() { }

        public UserLoginNotFoundException(string? message)
        : base(message) { }

        public UserLoginNotFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
}
