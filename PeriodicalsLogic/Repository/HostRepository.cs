using Periodicals.DAL.Accounts;
using Periodicals.DAL.DbHelpers;
using Periodicals.DAL.Publishings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Periodicals.DAL.Repository
{
    public class HostRepository : IRepository<Host>
    {
        public PeriodicalsContext db;

        public HostRepository(PeriodicalsContext context)
        {
            this.db = context;
        }

        public List<Host> GetAllHosts()
        {
            return db.Hosts.Where(user => user.Role == "User").ToList();
        }

        public List<Tag> GetAllTags()
        {
            return db.Tags.ToList();
        }
        public List<Host> GetAllAuthors()
        {
            return db.Hosts.Where(user => user.Role == "Author").ToList();
        }

        public List<Magazine> GetAllMagazines()
        {
            return db.Magazines.Include(magazine => magazine.Tags).ToList();
        }


        public Host GetHost(string email)
        {
            return db.Hosts.Find(email);
        }

        public Host GetHostById(int id)
        {
            return db.Hosts
                .Include(host => host.Magazines)
                .Single(host => host.Id == id);
        }

        public List<Magazine> GetUserMagazines(int id)
        {
            return db.Hosts
                .Include(host => host.Magazines)
                .Single(host => host.Id == id)
                .Magazines.ToList();
        }

        public Magazine GetMagazine(int? id)
        {
            return db.Magazines.Include(magazine => magazine.Tags).Single(magazine => magazine.MagazineId == id);
        }

        public Magazine GetMagazine(string name)
        {
            try
            {
                return db.Magazines.Include(magazine => magazine.Tags).Single(magazine => magazine.MagazineName == name);
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public Host GetByAuthenfication(string email, string password, string role)
        {
            try
            {
                return db.Hosts.Single(host => host.Email == email && host.Password == password && host.Role == role);
            }
            catch (Exception ex)
            {
                return null;
            }
            
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
            db.SaveChanges();
        }

        public void CreateHost(Host host)
        {
            db.Hosts.Add(host);
            db.SaveChanges();
        }

        public void CreateMagazine(Magazine magazine)
        {
            magazine.PublishDate = DateTime.Now;
            db.Magazines.Add(magazine);
            db.SaveChanges();
        }

        public void UpdateMagazine(Magazine magazine)
        {
            magazine.Cover = " ";
            magazine.PublishDate = DateTime.Now;
            db.Entry(magazine).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void UpdateHost(Host host)
        {
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    Host hostEdited = db.Hosts.Single(tempHost => tempHost.Id == host.Id);
                    hostEdited.Name = host.Name;
                    hostEdited.Wallet = host.Wallet;
                    hostEdited.Email = host.Email;
                    hostEdited.Password = host.Password;
                    db.Entry(hostEdited).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
        }

        public void DeleteUser(int id)
        {
            Host host = db.Hosts.Find(id);
            if (host != null)
            {
                db.Hosts.Remove(host);
                db.SaveChanges();
            }
                
        }

        public void DeleteMagazine(int? id)
        {
            Magazine magazine = db.Magazines.Find(id);
            if (magazine != null)
            {
                db.Magazines.Remove(magazine);
                db.SaveChanges();
            }   
        }

        public void Dispose()
        {
            db.Dispose();
        }
    }
}


        