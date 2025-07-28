namespace CoffeeShops.Domain.Models
{
    public class FavDrinks
    {
        public Guid Id_user { get; set; }
        public Guid Id_drink { get; set; }

        public FavDrinks() { }

        public FavDrinks(Guid _Id_user, Guid _Id_drink)
        {
            Id_user = _Id_user;
            Id_drink = _Id_drink;
        }
    }
}