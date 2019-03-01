using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using System.Security.Claims;
using Periodicals.DAL.UnitOfWork;
using Periodicals.DAL.Accounts;
using System;
using Periodicals.DAL.Publishings;

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

        public ClaimsIdentity Authenticate(HostDTO hostDto)
        {
            ClaimsIdentity claim = null;
            Host host = Database.Hosts.GetByAuthenfication(hostDto.Email, hostDto.Password);
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
            Host hostEdited = Database.Hosts.GetHost(host.Email);
            hostEdited.Name = host.Name;
            hostEdited.Password = host.Password;
            hostEdited.ProfilePicture = host.ProfilePicture;
            hostEdited.Vallet = host.Vallet;
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

        public void Dispose()
        {
            Database.Dispose();
        }

        public void SetInitialData(HostDTO host)
        {
            Host host1 = new Host() { Email = host.Email, Password = host.Password };
            Database.Hosts.CreateHost(host1);
        }


        OperationSatus IUserService.CreateMagasine(MagazineDTO magazineDTO, int authorId)
        {
            Magazine magazine = new Magazine()
            {
                MagazineName = magazineDTO.MagazineName,
                HostId = authorId,
                Cover = magazineDTO.Cover,
                Description = magazineDTO.Description,
                Tags = magazineDTO.Tags,
                Price = magazineDTO.Price
            };
            if (magazine != null)
            {
                Database.Hosts.CreateMagazine(magazine);
                return new OperationSatus(true, "Create was succsesfull", "");
            }
            else
            {
                return new OperationSatus(false, "Something went wrong", "MagazineCreate");
            }
        }

        OperationSatus IUserService.EditMagazine(MagazineDTO magazineDTO)
        {
            Magazine magazineEdited = Database.Hosts.GetMagazine(magazineDTO.Id);
            magazineEdited.MagazineName = magazineDTO.MagazineName;
            magazineEdited.Cover = magazineDTO.Cover;
            magazineEdited.Description = magazineDTO.Description;
            magazineEdited.Tags = magazineDTO.Tags;
            magazineEdited.Price = magazineDTO.Price;
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
            Host hostEdited = Database.Hosts.GetHost(userDTO.Email);
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

        public OperationSatus DeleteUserMagazine(HostDTO userDTO, int magasineId)
        {
            Host hostEdited = Database.Hosts.GetHost(userDTO.Email);
            hostEdited.Magazines.Remove(Database.Hosts.GetMagazine(magasineId));
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
    }
}
