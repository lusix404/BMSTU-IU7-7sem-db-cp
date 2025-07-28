namespace CoffeeShops.Domain.Exceptions.DrinkServiceExceptions
{
    public class NoCoffeeShopsFoundException : Exception
    {
        public NoCoffeeShopsFoundException() { }

        public NoCoffeeShopsFoundException(string? message)
        : base(message) { }

        public NoCoffeeShopsFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
