namespace CoffeeShops.Domain.Exceptions.CompanyServiceExceptions
{
    public class CompanyAlreadyExistsException : Exception
    {
        public CompanyAlreadyExistsException() { }

        public CompanyAlreadyExistsException(string? message)
        : base(message) { }

        public CompanyAlreadyExistsException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
}
