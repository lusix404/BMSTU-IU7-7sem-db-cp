using Microsoft.EntityFrameworkCore;
using System;

namespace CoffeeShops.DataAccess.Context;

public class AdminDbContext : CoffeeShopsContext
{
    public AdminDbContext(DbContextOptions<AdminDbContext> options)
    : base(options) { }
}