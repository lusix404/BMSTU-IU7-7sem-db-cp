namespace CoffeeShops.Domain.Models
{
    public class LoyaltyProgram
    {
        public Guid Id_lp { get; set; }
        public Guid Id_company { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public LoyaltyProgram() { }
        public LoyaltyProgram(Guid _Id_lp, Guid _Id_company, string? _Type, string? _Description)
        {
            this.Id_lp = _Id_lp;
            this.Id_company = _Id_company;
            this.Description = _Description;
            this.Type = _Type;
        }
        public LoyaltyProgram(Guid _Id_company, string _Type,string _Description)
        {
            this.Id_company = _Id_company;
            this.Description = _Description;
            this.Type = _Type;
        }
    }
}