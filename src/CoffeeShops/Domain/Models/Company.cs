using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace CoffeeShops.Domain.Models
{
    public class Company
    {
        public Guid Id_company { get; set; }
        public string Name { get; set; }

        public string Website { get; set; }

        public int AmountCoffeeShops { get; set; }

        public Company() { }
        public Company(Guid _Id_company, string _Name, string _Website, int _AmountCoffeeShops)
        {
            this.Id_company = _Id_company;
            this.Name = _Name;
            this.Website = _Website;
            this.AmountCoffeeShops = _AmountCoffeeShops;
        }
        public Company(string _Name, string _Website, int _AmountCoffeeShops)
        {
            this.Name = _Name;
            this.Website = _Website;
            this.AmountCoffeeShops = _AmountCoffeeShops;
        }

        protected bool IsValidWebsite(string website)
        {
            return Regex.IsMatch(website, @"^(https?:\/\/)?(www\.)?([a-zA-Z0-9-]+\.)*[a-zA-Z0-9-]+\.(ru|com|net|org)(\/[^\s]*)?$");
        }

        protected bool IsValidAmountCOffeeShops(int amount)
        {
            return amount >= 0;
        }

        public bool Validate()
        {
            if (this.Website != null)
            {
                return IsValidWebsite(this.Website) & IsValidAmountCOffeeShops(this.AmountCoffeeShops);
            }
            return IsValidAmountCOffeeShops(this.AmountCoffeeShops);
        }
    }
}