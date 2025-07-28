namespace CoffeeShops.Domain.Exceptions.DrinkServiceExceptions
{
    public class DrinkIncorrectAtributeException : Exception
    {
        public DrinkIncorrectAtributeException() { }

        public DrinkIncorrectAtributeException(string? message)
        : base(message) { }

        public DrinkIncorrectAtributeException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
