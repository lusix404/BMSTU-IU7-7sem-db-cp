namespace CoffeeShops.Domain.Models
{
    public class Role
    {
        public int Id_role { get; set; }

        public string Name { get; set; }

        public Role() { }

        public Role (int _Id_role, string _Name)
        {
            this.Id_role = _Id_role;
            this.Name = _Name;
        }
    }
}
