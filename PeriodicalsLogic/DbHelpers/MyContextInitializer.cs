using Periodicals.DAL.Accounts;
using Periodicals.DAL.Publishings;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Periodicals.DAL.DbHelpers
{
    public class MyContextInitializer : DropCreateDatabaseAlways<PeriodicalsContext>
    {
        protected override void Seed(PeriodicalsContext db)
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
                Role = "Author",
                Name = "Emily Clark",
                Password = "clark",
                Email = "clark@gmail.com"
            };

            Host host4 = new Host()
            {
                Id = 4,
                Role = "Author",
                Name = "Amily Hatson",
                Password = "amily",
                Email = "amily@gmail.com"
            };

            Magazine magazine1 = new Magazine()
            {
                MagazineId = 1,
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

            Magazine magazine2 = new Magazine()
            {
                MagazineId = 2,
                MagazineName = "National Geographic",
                HostId = 4,
                Description = "National Geographic(formerly the National Geographic Magazine and branded also as NAT GEO) " +
                "is the official magazine of the National Geographic Society.It has been published continuously since its first issue in 1888," +
                "nine months after the Society itself was founded.It primarily contains articles about science," +
                "geography, history,and world culture. The magazine is known for its thick square " +
                "- bound glossy format with a yellow rectangular border and its extensive use of dramatic photographs.Controlling interest in the magazine has been held by 21st Century Fox since 2015.",
                Price = 400,
                Tags = new List<Tag>() { tag5, tag9 },
                PublishDate = DateTime.Now
            };

            host1.Magazines.Add(magazine1);
            host4.Magazines.Add(magazine2);
            Host host2 = new Host()
            {
                Id = 0,
                Role = "User",
                Name = "Yuliia Zelenska",
                ProfilePicture = "/Content/images/profileTest.jpg",
                Email = "detre39@gmail.com",
                Magazines = new List<Magazine>() { magazine1 },
                Wallet = 1000,
                Password = "detre39"
            };

            magazine1.Hosts.Add(host2);
            host2.Magazines.Add(magazine1);

            Host host3 = new Host()
            {
                Id = 3,
                Name = "admin",
                Role = "Admin",
                Email = "admin@gmail.com",
                Password = "admin"
            };


            db.Hosts.AddRange(new List<Host>() { host2, host1, host3, host4 });
            db.Tags.AddRange(new List<Tag>() { tag1, tag2, tag3, tag4, tag5, tag6, tag7, tag8, tag9, tag10 });
            db.Magazines.AddRange(new List<Magazine>() { magazine1, magazine2 });
        }
    }
}
