using System;

namespace CoffeeShops.Domain.Exceptions.CategoryServiceExceptions
{
    public class NoCategoriesInDataBase : Exception
    {
        public NoCategoriesInDataBase() { }

        public NoCategoriesInDataBase(string? message)
        : base(message) { }

        public NoCategoriesInDataBase(string? message, Exception? innerException)
        : base(message, innerException) { }
    }


}
