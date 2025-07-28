namespace CoffeeShops.Domain.Models
{
    public class Category
    {
        public Guid Id_category { get; set; }
        public string Name { get; set; }

        public Category() { }   

        public Category(Guid _Id, string _Name)
        {
            this.Id_category = _Id;
            this.Name = _Name;
        }
        public Category(string _Name)
        {
            this.Name = _Name;
        }




    }


}