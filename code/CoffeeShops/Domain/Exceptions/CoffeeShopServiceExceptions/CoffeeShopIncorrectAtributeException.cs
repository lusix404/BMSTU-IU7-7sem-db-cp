namespace CoffeeShops.Domain.Exceptions.CoffeeShopServiceExceptions
{
    public class CoffeeShopIncorrectAtributeException : Exception
    {
        public CoffeeShopIncorrectAtributeException() { }

        public CoffeeShopIncorrectAtributeException(string? message)
        : base(message) { }

        public CoffeeShopIncorrectAtributeException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
}
