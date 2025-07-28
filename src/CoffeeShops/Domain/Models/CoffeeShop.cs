namespace CoffeeShops.Domain.Models
{
    public class CoffeeShop
    {
        public Guid Id_coffeeshop { get; set; }
        public Guid Id_company { get; set; }
        public string Address { get; set; }
        public string WorkingHours { get; set; }

        public CoffeeShop() { }
        public CoffeeShop(Guid _Id_coffeeshop, Guid _Id_company, string _Address, string _WorkingHours)
        {
            this.Id_coffeeshop = _Id_coffeeshop;
            this.Id_company = _Id_company;
            this.Address = _Address;
            this.WorkingHours = _WorkingHours;
        }
        public CoffeeShop(Guid _Id_company, string _Address, string _WorkingHours)
        {
            this.Id_company = _Id_company;
            this.Address = _Address;
            this.WorkingHours = _WorkingHours;
        }
    }
}