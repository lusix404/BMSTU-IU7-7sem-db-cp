namespace CoffeeShops.Domain.Exceptions.LoyaltyProgramServiceExceptions;

    public class LoyaltyProgramIncorrectAtributeException : Exception
    {
        public LoyaltyProgramIncorrectAtributeException() { }

        public LoyaltyProgramIncorrectAtributeException(string? message)
        : base(message) { }

        public LoyaltyProgramIncorrectAtributeException(string? message, Exception? innerException)
        : base(message, innerException) { }
    }
