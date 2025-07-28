namespace CoffeeShops.Domain.Exceptions.CoffeeShopServiceExceptions
{
    public class CoffeeShopNotFoundException : Exception
    {
        public CoffeeShopNotFoundException() { }

        public CoffeeShopNotFoundException(string? message)
        : base(message) { }

        public CoffeeShopNotFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
}
