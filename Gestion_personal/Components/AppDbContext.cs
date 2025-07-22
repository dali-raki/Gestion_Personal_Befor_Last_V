using Microsoft.EntityFrameworkCore;

namespace Gestion_personal.Components
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        
        }

        public DbSet<UserAccount> UserAccounts {  get; set; }
    }
}
