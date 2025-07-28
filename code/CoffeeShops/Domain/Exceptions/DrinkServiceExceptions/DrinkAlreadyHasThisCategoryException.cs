namespace CoffeeShops.Domain.Exceptions.DrinkServiceExceptions
{
    public class DrinkAlreadyHasThisCategoryException : Exception
    {
        public DrinkAlreadyHasThisCategoryException() { }

        public DrinkAlreadyHasThisCategoryException(string? message)
        : base(message) { }

        public DrinkAlreadyHasThisCategoryException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
