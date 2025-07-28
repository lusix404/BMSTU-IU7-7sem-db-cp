namespace CoffeeShops.Domain.Exceptions.DrinkServiceExceptions
{
    public class DrinkNotFoundException : Exception
    {
        public DrinkNotFoundException() { }

        public DrinkNotFoundException(string? message)
        : base(message) { }

        public DrinkNotFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
