using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ef_core_sqlite
{
  class Program
  {
    static async Task Main()
    {
      using (var appDbContext = new AppDbContext())
      {
        await appDbContext.Database.EnsureDeletedAsync();
        await appDbContext.Database.EnsureCreatedAsync();
        await appDbContext.Database.MigrateAsync();

        await appDbContext.Users.AddAsync(new User { Name = "Tom", Items = new[] { new Item { Name = "Tom's item" } } });
        await appDbContext.SaveChangesAsync();
      }

      using (var appDbContext = new AppDbContext())
      {
        var item = await appDbContext.Items.SingleAsync();
        Console.WriteLine(item.Id + " " + item.Name + " " + item.UserId);
      }
    }
  }

  class AppDbContext : DbContext
  {
    public DbSet<Item> Items { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      optionsBuilder.UseSqlite($"Data Source={nameof(ef_core_sqlite)}.db");
      optionsBuilder.EnableDetailedErrors();
      optionsBuilder.EnableSensitiveDataLogging();
    }
  }

  class Item
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
  }

  class User
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<Item> Items { get; set; }
  }
}
