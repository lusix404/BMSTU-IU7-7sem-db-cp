namespace CoffeeShops.Domain.Exceptions.DrinkServiceExceptions
{
    public class DrinkAlreadyIsFavoriteException : Exception
    {
        public DrinkAlreadyIsFavoriteException() { }

        public DrinkAlreadyIsFavoriteException(string? message)
        : base(message) { }

        public DrinkAlreadyIsFavoriteException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
