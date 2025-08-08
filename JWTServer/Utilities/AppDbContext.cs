using JWTServer.Model;
using Microsoft.EntityFrameworkCore;

namespace JWTServer.Utilities
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
        public DbSet<UserLogin> Users { get; set; }
    }
}
