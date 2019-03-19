using Microsoft.AspNet.Identity.EntityFramework;
using Periodicals.DAL.Accounts;
using Periodicals.DAL.Publishings;
using System.Data.Entity;

namespace Periodicals.DAL.DbHelpers
{
    /// <summary>
    ///  Class creates database and DbSets, with which will work
    /// </summary>
    
    public class PeriodicalsContext : IdentityDbContext, IDbContext
    {
        public DbSet<Host> Hosts { get; set; }

        public DbSet<Magazine> Magazines { get; set; }

        public DbSet<Tag> Tags { get; set; }

        public PeriodicalsContext(string conectionString) 
             : base(conectionString)
        {
            Database.SetInitializer(new MyContextInitializer());
        }

        public PeriodicalsContext() { }

        /// <summary>
        ///  Methods initialize foreing keys for connection to othe tables
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Host>()
                .HasMany(p => p.Magazines)
                .WithMany(c => c.Hosts)
                .Map(x =>
                {
                    x.MapLeftKey("HostId");
                    x.MapRightKey("MagazineId");
                    x.ToTable("HostMagazine");
                });

            modelBuilder.Entity<Magazine>()
                .HasMany(p => p.Tags)
                .WithMany(c => c.Magazines)
                .Map(x =>
                {
                    x.MapLeftKey("MagazineId");
                    x.MapRightKey("TagId");
                    x.ToTable("TagMagazine");
                });

            modelBuilder.Entity<IdentityUserLogin>().HasKey(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        }
    }
}
