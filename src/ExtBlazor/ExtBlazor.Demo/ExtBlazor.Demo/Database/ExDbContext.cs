using ExtBlazor.Demo.Client.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtBlazor.Demo.Database;

public class ExDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
