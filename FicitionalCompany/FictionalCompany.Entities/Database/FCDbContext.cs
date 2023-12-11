using FictionalCompany.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace FictionalCompany.Entities.Database
{
    public class FCDbContext: DbContext
    {
        private readonly IConfiguration _config;
        public DbSet<User> Users { get; set; }

        public FCDbContext(DbContextOptions<FCDbContext> options,IConfiguration config)
            : base(options)
        {
            _config = config;
            Database.SetCommandTimeout(150000);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
