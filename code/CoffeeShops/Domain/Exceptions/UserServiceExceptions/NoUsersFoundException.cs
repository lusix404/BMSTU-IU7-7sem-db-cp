namespace CoffeeShops.Domain.Exceptions.UserServiceExceptions
{
    public class NoUsersFoundException : Exception
    {
        public NoUsersFoundException() { }

        public NoUsersFoundException(string? message)
        : base(message) { }

        public NoUsersFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
}
