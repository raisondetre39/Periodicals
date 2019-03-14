using System.Data.Entity.Migrations;

namespace Periodicals.DAL.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DbHelpers.PeriodicalsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DbHelpers.PeriodicalsContext context) { }
    }
}
