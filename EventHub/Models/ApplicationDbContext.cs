using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EventHub.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //if I want to query lists of some model, I need to add it here

        public DbSet<Event> Events { get; set; }
        public DbSet<Genre> Genres { get; set; }
         

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}