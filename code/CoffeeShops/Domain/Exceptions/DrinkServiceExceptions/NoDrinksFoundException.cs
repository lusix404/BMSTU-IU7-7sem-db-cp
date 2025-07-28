namespace CoffeeShops.Domain.Exceptions.DrinkServiceExceptions
{
    public class NoDrinksFoundException : Exception
    {
        public NoDrinksFoundException() { }

        public NoDrinksFoundException(string? message)
        : base(message) { }

        public NoDrinksFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
