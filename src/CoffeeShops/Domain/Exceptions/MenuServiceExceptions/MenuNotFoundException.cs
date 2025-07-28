namespace CoffeeShops.Domain.Exceptions.MenuServiceExceptions
{
    public class MenuNotFoundException : Exception
    {
        public MenuNotFoundException() { }

        public MenuNotFoundException(string? message)
        : base(message) { }

        public MenuNotFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
