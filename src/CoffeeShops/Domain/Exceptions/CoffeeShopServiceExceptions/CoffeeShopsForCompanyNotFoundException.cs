namespace CoffeeShops.Domain.Exceptions.CoffeeShopServiceExceptions
{
    public class CoffeeShopsForCompanyNotFoundException : Exception
    {
        public CoffeeShopsForCompanyNotFoundException() { }

        public CoffeeShopsForCompanyNotFoundException(string? message)
        : base(message) { }

        public CoffeeShopsForCompanyNotFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
}
