namespace CoffeeShops.Domain.Exceptions.MenuServiceExceptions
{
    public class MenuIncorrectAtributeException : Exception
    {
        public MenuIncorrectAtributeException() { }

        public MenuIncorrectAtributeException(string? message)
        : base(message) { }

        public MenuIncorrectAtributeException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
