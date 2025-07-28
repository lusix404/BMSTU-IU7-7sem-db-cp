using Microsoft.EntityFrameworkCore;
using System;

namespace CoffeeShops.DataAccess.Context;

public class UserDbContext : CoffeeShopsContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
    : base(options) { }
}