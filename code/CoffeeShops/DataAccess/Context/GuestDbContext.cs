using Microsoft.EntityFrameworkCore;
using System;

namespace CoffeeShops.DataAccess.Context;

public class GuestDbContext : CoffeeShopsContext
{
    public GuestDbContext(DbContextOptions<GuestDbContext> options)
    : base(options) { }
}