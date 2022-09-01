using Microsoft.EntityFrameworkCore;

namespace Observability.Service.A.Domain;

public class DataContext : DbContext
{
    public DbSet<DataItem> DataItems { get; private set; }
    public IConfiguration Configuration { get; set; }

    public DataContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<DataItem>().HasKey(x => x.Id);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(Configuration.GetConnectionString("DataContext"));
    }
}