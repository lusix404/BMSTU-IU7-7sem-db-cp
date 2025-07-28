using Microsoft.EntityFrameworkCore;
using System;

namespace CoffeeShops.DataAccess.Context;

public class ModerDbContext : CoffeeShopsContext
{
    public ModerDbContext(DbContextOptions<ModerDbContext> options)
    : base(options) { }
}