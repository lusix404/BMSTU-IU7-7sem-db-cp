namespace CoffeeShops.Domain.Exceptions.CompanyServiceExceptions
{
    public class NoCompaniesFoundException : Exception
    {
        public NoCompaniesFoundException() { }

        public NoCompaniesFoundException(string? message)
        : base(message) { }

        public NoCompaniesFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
}
