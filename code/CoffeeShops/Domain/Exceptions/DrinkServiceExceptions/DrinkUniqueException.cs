namespace CoffeeShops.Domain.Exceptions.DrinkServiceExceptions
{
    public class DrinkUniqueException : Exception
    {
        public DrinkUniqueException() { }

        public DrinkUniqueException(string? message)
        : base(message) { }

        public DrinkUniqueException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
