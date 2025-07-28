namespace CoffeeShops.Domain.Models
{
    public class Menu
    {
        public Guid Id_menu { get; set; }
        public Guid Id_drink { get; set; }
        public Guid Id_company { get; set; }
        public int Size { get; set; }
        public decimal Price { get; set; }

        public Menu() { }
        public Menu(Guid _Id_menu, Guid _Id_drink, Guid _Id_company, int _Size, decimal _Price)
        {
            Id_menu = _Id_menu;
            Id_drink = _Id_drink;
            Id_company = _Id_company;
            Size = _Size;
            Price = _Price;
        }
        public Menu(Guid _Id_drink, Guid _Id_company, int _Size, decimal _Price)
        {
            Id_drink = _Id_drink;
            Id_company = _Id_company;
            Size = _Size;
            Price = _Price;
        }
    }
}