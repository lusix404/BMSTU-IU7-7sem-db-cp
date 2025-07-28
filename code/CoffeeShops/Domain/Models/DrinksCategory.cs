namespace CoffeeShops.Domain.Models
{
    public class DrinksCategory
    {
        public Guid Id_drink { get; set; }
        public Guid Id_category { get; set; }

        public DrinksCategory() { }
        public DrinksCategory (Guid _Id_drink, Guid _Id_category)
        {
            Id_drink = _Id_drink;
            Id_category = _Id_category;
        }
    }
}