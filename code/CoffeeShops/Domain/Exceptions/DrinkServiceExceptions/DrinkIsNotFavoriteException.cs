namespace CoffeeShops.Domain.Exceptions.DrinkServiceExceptions
{
    public class DrinkIsNotFavoriteException : Exception
    {
        public DrinkIsNotFavoriteException() { }

        public DrinkIsNotFavoriteException(string? message)
        : base(message) { }

        public DrinkIsNotFavoriteException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
