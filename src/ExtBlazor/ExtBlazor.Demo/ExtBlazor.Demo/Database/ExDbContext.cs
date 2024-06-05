using ExtBlazor.Demo.Client.Models;
using Microsoft.EntityFrameworkCore;

namespace ExtBlazor.Demo.Database;

public class ExDbContext : DbContext
{
    public ExDbContext(DbContextOptions options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
