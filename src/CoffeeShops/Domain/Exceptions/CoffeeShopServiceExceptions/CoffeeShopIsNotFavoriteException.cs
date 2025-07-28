namespace CoffeeShops.Domain.Exceptions.DrinkServiceExceptions
{
    public class CoffeeShopIsNotFavoriteException : Exception
    {
        public CoffeeShopIsNotFavoriteException() { }

        public CoffeeShopIsNotFavoriteException(string? message)
        : base(message) { }

        public CoffeeShopIsNotFavoriteException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
