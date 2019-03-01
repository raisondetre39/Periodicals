using Periodicals.DAL.Accounts;
using Periodicals.DAL.Publishings;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Periodicals.DAL.DbHelpers
{
    public class MyContextInitializer : DropCreateDatabaseAlways<PeriodicalContext>
    {
        protected override void Seed(PeriodicalContext db)
        {
            Tag tag1 = new Tag() { TagName = "social" };
            Tag tag2 = new Tag() { TagName = "fashion" };
            Tag tag3 = new Tag() { TagName = "culture" };
            Tag tag4 = new Tag() { TagName = "business" };
            Tag tag5 = new Tag() { TagName = "enviroment" };
            Tag tag6 = new Tag() { TagName = "politics" };
            Tag tag7 = new Tag() { TagName = "study" };
            Tag tag8 = new Tag() { TagName = "sience" };
            Tag tag9 = new Tag() { TagName = "animal" };
            Tag tag10 = new Tag() { TagName = "films" };

            Host host1 = new Host()
            {
                Id = 2,
                Name = "Emily Clark",
                Password = "clark",
                Email = "clark@gmail.com"
            };

            Magazine magazine1 = new Magazine()
            {
                MagazineName = "Times",
                HostId = 2,
                Description = "The Times is a British daily (Monday to Saturday)" +
                " national newspaper based in London. It began in 1785 under the title The Daily Universal Register, " +
                "adopting its current name on 1 January 1788. The Times and its sister paper" +
                " The Sunday Times (founded in 1821) are published by Times Newspapers, since 1981 " +
                "a subsidiary of News UK, itself wholly owned by News Corp." +
                " The Times and The Sunday Times do not share editorial staff, were founded" +
                " independently, and have only had common ownership since 1967.",
                Price = 300,
                Tags = new List<Tag>() { tag1, tag2 },
                PublishDate = DateTime.Now
            };

            host1.Magazines.Add(magazine1);

            Host host2 = new Host()
            {
                Id = 1,
                Name = "Yuliia Zelenska",
                Email = "detre39@gmail.com",
                Magazines = new List<Magazine>() { magazine1 },
                Vallet = 1000,
                Password = "detre39"
            };

            Host host3 = new Host()
            {
                Id = 3,
                Name = "admin",
                Email = "admin@gmail.com",
                Password = "admin"
            };


            db.Hosts.AddRange(new List<Host>() { host2, host1, host3 });
            db.Tags.AddRange(new List<Tag>() { tag1, tag2 });
            db.Magazines.Add(magazine1);
            
        }
    }
}
