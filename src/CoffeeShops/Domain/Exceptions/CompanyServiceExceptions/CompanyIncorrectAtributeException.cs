namespace CoffeeShops.Domain.Exceptions.CompanyServiceExceptions;

    public class CompanyIncorrectAtributeException : Exception
    {
        public CompanyIncorrectAtributeException() { }

        public CompanyIncorrectAtributeException(string? message)
        : base(message) { }

        public CompanyIncorrectAtributeException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
