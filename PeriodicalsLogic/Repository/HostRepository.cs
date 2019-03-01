using Periodicals.DAL.Accounts;
using Periodicals.DAL.DbHelpers;
using Periodicals.DAL.Publishings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Periodicals.DAL.Repository
{
    public class HostRepository : IRepository<Host>
    {
        private PeriodicalContext db;

        public HostRepository(PeriodicalContext context)
        {
            this.db = context;
        }

        public IEnumerable<Host> GetAllHosts()
        {
            return db.Hosts.Where(user => user.Role == "User");
        }

        public IEnumerable<Host> GetAllAuthors()
        {
            return db.Hosts.Where(user => user.Role == "Author");
        }

        public IEnumerable<Magazine> GetAllMagazines()
        {
            return db.Magazines;
        }

        public Host GetHost(string email)
        {
            return db.Hosts.Find(email);
        }

        public Magazine GetMagazine(int id)
        {
            return db.Magazines.Find(id);
        }

        public Host GetByAuthenfication(string email, string password)
        {
            return db.Hosts.Single(host => host.Email == email && host.Password == password);
        }

        public void CreateHost(Host host, string password)
        {
            host.Role = "User";
            host.Password = password;
            db.Hosts.Add(host);
        }

        public void CreateAuthor(Host host, string password)
        {
            host.Role = "Author";
            host.Password = password;
            db.Hosts.Add(host);
        }

        public void CreateHost(Host host)
        {
            db.Hosts.Add(host);
        }

        public void CreateMagazine(Magazine magazine)
        {
            magazine.PublishDate = DateTime.Now;
            db.Magazines.Add(magazine);
        }

        public void UpdateMagazine(Magazine magazine)
        {
            db.Entry(magazine).State = EntityState.Modified;
        }

        public void UpdateHost(Host host)
        {
            db.Entry(host).State = EntityState.Modified;
        }

        public void DeleteUser(int id)
        {
            Host host = db.Hosts.Find(id);
            if (host != null)
                db.Hosts.Remove(host);
        }

        public void DeleteMagazine(int id)
        {
            Magazine magazine = db.Magazines.Find(id);
            if (magazine != null)
                db.Magazines.Remove(magazine);
        }

    }
}


        