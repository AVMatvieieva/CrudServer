using Microsoft.EntityFrameworkCore;


namespace CrudServer.Model
{
    public class UserContext:DbContext
    {
        public DbSet<User> Users { get; set; } 
        
        public UserContext(DbContextOptions<UserContext> options):base(options)
        {
            Database.EnsureCreated();
        }
        
    }
}
