namespace CoffeeShops.Domain.Models
{
    public class FavCoffeeShops
    {
        public Guid Id_user { get; set; }
        public Guid Id_coffeeshop { get; set; }

        public FavCoffeeShops() { }
        public FavCoffeeShops(Guid _Id_user, Guid _Id_coffeeshop)
        {
            Id_user = _Id_user;
            Id_coffeeshop = _Id_coffeeshop;
        }
    }
}