using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using System.Security.Claims;
using Periodicals.DAL.UnitOfWork;
using Periodicals.DAL.Accounts;
using System;
using Periodicals.DAL.Publishings;
using System.Collections.Generic;
using System.Linq;

namespace Periodical.BL.Services
{
    public class HostService : IUserService
    {
        UnitOfWork Database { get; set; }

        public HostService(UnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }
        public OperationSatus CreateUser(HostDTO hostDto, string role)
        {
            Host host = Database.Hosts.GetHost(hostDto.Email);
            if (host == null)
            {
                host = new Host { Email = hostDto.Email, Name = hostDto.Email };
                if(role == "User")
                {
                    Database.Hosts.CreateHost(host, hostDto.Password);
                }
                else if(role == "Author")
                {
                    Database.Hosts.CreateAuthor(host, hostDto.Password);
                }
                return new OperationSatus(true, "Registration was sucssesful", "");
            }
            else
            {
                return new OperationSatus(false, "User with this name alredy exsits", "Email");
            }
        }

        public ClaimsIdentity Authenticate(HostDTO hostDto, string role)
        {
            ClaimsIdentity claim = null;
            Host host = Database.Hosts.GetByAuthenfication(hostDto.Email, hostDto.Password, role);
            if (host != null)
            {
                claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, host.Id.ToString(), ClaimValueTypes.String));
                claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, host.Email, ClaimValueTypes.String));
                claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                    "OWIN Provider", ClaimValueTypes.String));
            }
            return claim;
        }

        public OperationSatus EditUser(HostDTO host)
        {
            if (host != null)
            {
                Database.Hosts.UpdateHost(HostDTO.ToHost(host));
                return new OperationSatus(true, "Changes were succsesfull", "");
            }
            else
            {
                return new OperationSatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public void SetInitialData(HostDTO host)
        {
            Host host1 = new Host() { Email = host.Email, Password = host.Password };
            Database.Hosts.CreateHost(host1);
        }


        public OperationSatus CreateMagazine(MagazineDTO magazineDTO, HostDTO author)
        {
            Magazine magazine = MagazineDTO.ToMagazine(magazineDTO, author.Id);
            if (magazine != null)
            {
                Database.Hosts.CreateMagazine(magazine);
                AddUserMagazine(author, magazine.MagazineId);

                return new OperationSatus(true, "Create was succsesfull", "");
            }
            else
            {
                return new OperationSatus(false, "Something went wrong", "MagazineCreate");
            }
        }

        public OperationSatus EditMagazine(MagazineDTO magazineDTO)
        {
            Magazine magazineEdited = MagazineDTO.ToMagazine(magazineDTO, magazineDTO.HostId);
            if (magazineEdited != null)
            {
                Database.Hosts.UpdateMagazine(magazineEdited);

                return new OperationSatus(true, "Update was succsesfull", "");
            }
            else
            {
                return new OperationSatus(false, "Something went wrong", "MagazineUpdate");
            }
        }

        public OperationSatus AddUserMagazine(HostDTO userDTO, int magasineId)
        {
            Host hostEdited = Database.Hosts.GetHostById(userDTO.Id);
            hostEdited.Magazines.Add(Database.Hosts.GetMagazine(magasineId));
            if (hostEdited != null)
            {
                Database.Hosts.UpdateHost(hostEdited);
                return new OperationSatus(true, "Changes were succsesfull", "");
            }
            else
            {
                return new OperationSatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public OperationSatus DeleteUserMagazine(HostDTO userDTO, int? magasineId)
        {
            Host hostEdited = Database.Hosts.GetHostById(userDTO.Id);
            hostEdited.Magazines.Remove(Database.Hosts.GetMagazine(magasineId));
            Magazine magazine = Database.Hosts.GetMagazine(magasineId);
            magazine.Hosts.Remove(hostEdited);
            if (hostEdited != null)
            {
                Database.Hosts.UpdateHost(hostEdited);
                Database.Hosts.UpdateMagazine(magazine);
                return new OperationSatus(true, "Changes were succsesfull", "");
            }
            else
            {
                return new OperationSatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public OperationSatus BlockUser(string email)
        {
            Host hostEdited = Database.Hosts.GetHost(email);
            hostEdited.IsBlocked = true;
            if (hostEdited != null)
            {
                Database.Hosts.UpdateHost(hostEdited);
                return new OperationSatus(true, "Changes were succsesfull", "");
            }
            else
            {
                return new OperationSatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public OperationSatus UnlockUser(string email)
        {
            Host hostEdited = Database.Hosts.GetHost(email);
            hostEdited.IsBlocked = false;
            if (hostEdited != null)
            {
                Database.Hosts.UpdateHost(hostEdited);
                return new OperationSatus(true, "Changes were succsesfull", "");
            }
            else
            {
                return new OperationSatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public HostDTO GetHost(string email)
        {
            return HostDTO.ToHostDTO(Database.Hosts.GetHost(email));
        }

        public MagazineDTO GetMagazine(int? id)
        {
            Magazine magazine = Database.Hosts.GetMagazine(id);
            MagazineDTO magazineDTO = MagazineDTO.ToMagazineDTO(magazine);
            return magazineDTO;
        }

        public MagazineDTO GetMagazine(string name)
        {
            Magazine magazine = Database.Hosts.GetMagazine(name);
            if(magazine != null)
            {
                MagazineDTO magazineDTO = MagazineDTO.ToMagazineDTO(magazine);
                return magazineDTO;
            }
            return null;
        }

        public IEnumerable<HostDTO> GetAllHosts()
        {
            return Database.Hosts.GetAllHosts()
                .Select(host => HostDTO.ToHostDTO(host));
        }

        public IEnumerable<HostDTO> GetAllAuthors()
        {
            return Database.Hosts.GetAllAuthors()
                .Select(host => HostDTO.ToHostDTO(host));
        }

        public IEnumerable<Tag> GetAllTags()
        {
            return Database.Hosts.GetAllTags();
        }

        public IEnumerable<MagazineDTO> GetAllMagazines()
        {
            return Database.Hosts.GetAllMagazines()
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine));
        }

        public HostDTO GetHostById(int id)
        {
            HostDTO hostDTO =  HostDTO.ToHostDTO(Database.Hosts.GetHostById(id));
            hostDTO.Magazines = Database.Hosts.GetUserMagazines(id)
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine))
                .ToList();
            return hostDTO;
        }

        public OperationSatus DeleteMagazine(int? id)
        {
            Magazine magazine = Database.Hosts.GetMagazine(id);
            Database.Hosts.DeleteMagazine(id);
            return new OperationSatus(true, "Delete was succsesfull", "");
        }
    }
}
