namespace CoffeeShops.Domain.Exceptions.CoffeeShopServiceExceptions
{
    public class CoffeeShopAlreadyIsFavoriteException : Exception
    {
        public CoffeeShopAlreadyIsFavoriteException() { }

        public CoffeeShopAlreadyIsFavoriteException(string? message)
        : base(message) { }

        public CoffeeShopAlreadyIsFavoriteException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
