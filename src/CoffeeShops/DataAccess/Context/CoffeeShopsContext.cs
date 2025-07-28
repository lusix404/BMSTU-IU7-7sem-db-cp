using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CoffeeShops.DataAccess.Models;

using Microsoft.EntityFrameworkCore.Metadata.Builders; 



namespace CoffeeShops.DataAccess.Context;

public class CoffeeShopsContext : DbContext
{


    public CoffeeShopsContext(DbContextOptions options) : base(options)
    {
    }
    public CoffeeShopsContext(DbContextOptions<CoffeeShopsContext> options) : base(options)
    {
    }
}