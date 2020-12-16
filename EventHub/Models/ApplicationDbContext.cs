using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace EventHub.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //if I want to query lists of some model, I need to add it here

        public DbSet<Event> Events { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Following> Followings { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }


        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //CONFIGURATION between classes
            //to prevent cascade delete in one path (unnecesary if we delete an event)
            modelBuilder.Entity<Attendance>()
                .HasRequired(a => a.Event)
                .WithMany(e => e.Attendances)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Followers)
                .WithRequired(f => f.Followee)
                .WillCascadeOnDelete(false);    //because of multi cascade path problem

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Followees)
                .WithRequired(f => f.Follower)  //each followee has a required follower
                .WillCascadeOnDelete(false);

            //error between UserNotifications and AspNetUsers
            modelBuilder.Entity<UserNotification>()
                .HasRequired(un => un.User)
                .WithMany(u => u.UserNotifications)     //notifications (but no nav prop in the class)
                .WillCascadeOnDelete(false);


            base.OnModelCreating(modelBuilder);
        }
    }
}