namespace CoffeeShops.Domain.Exceptions.CompanyServiceExceptions
{
    public class CompaniesByDrinkNotFoundException : Exception
    {
        public CompaniesByDrinkNotFoundException() { }

        public CompaniesByDrinkNotFoundException(string? message)
        : base(message) { }

        public CompaniesByDrinkNotFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
}
