using E_ranga.Models;
using Microsoft.EntityFrameworkCore;

namespace E_ranga.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Documents> documents { get; set; }
        public DbSet<Users> users { get; set; }
    }

}