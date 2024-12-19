using ConceptZeeWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConceptZeeWebAPI
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<CZeeContact> Contacts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=czeecontacts.db");
            }
        }
    }
}

