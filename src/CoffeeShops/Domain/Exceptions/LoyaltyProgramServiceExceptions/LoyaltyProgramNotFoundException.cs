namespace CoffeeShops.Domain.Exceptions.LoyaltyProgramServiceExceptions
{
    public class LoyaltyProgramNotFoundException : Exception
    {
        public LoyaltyProgramNotFoundException() { }

        public LoyaltyProgramNotFoundException(string? message)
        : base(message) { }

        public LoyaltyProgramNotFoundException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
    
}
