using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Inventory_API.Data
{
    public class RestContext : DbContext
    {
        public RestContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; private set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<List> Lists { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ListItem> ListItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity => { entity.HasIndex(e => e.Username).IsUnique(); });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder) => dbContextOptionsBuilder.UseSqlServer(Configuration.GetConnectionString("MainDBConnection"));

    }
}
