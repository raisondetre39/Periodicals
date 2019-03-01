namespace Periodicals.DAL.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Periodicals.DAL.DbHelpers.PeriodicalContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Periodicals.DAL.DbHelpers.PeriodicalContext context)
        { }
    }
}
