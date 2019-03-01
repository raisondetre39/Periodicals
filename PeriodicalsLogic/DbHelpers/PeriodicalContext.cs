using Microsoft.AspNet.Identity.EntityFramework;
using Periodicals.DAL.Accounts;
using Periodicals.DAL.Publishings;
using System.Data.Entity;

namespace Periodicals.DAL.DbHelpers
{
    public class PeriodicalContext : IdentityDbContext
    {
        public PeriodicalContext(string conectionString) : base(conectionString)
        {
            Database.SetInitializer(new MyContextInitializer());
        }

        public PeriodicalContext()
        {
            
        }
        
        public DbSet<Host> Hosts { get; set; }
        public DbSet<Magazine> Magazines { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }
}
