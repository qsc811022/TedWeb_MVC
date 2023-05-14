using Microsoft.EntityFrameworkCore;

using TedWeb.Models;

namespace TedWeb.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        
        }
        public DbSet<Category> Categories { get; set; }
    }
}
