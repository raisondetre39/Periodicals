using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using System.Security.Claims;
using Periodicals.DAL.UnitOfWork;
using Periodicals.DAL.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace Periodical.BL.Services
{
    public class HostService : IHostService
    {
        UnitOfWork Database { get; set; }

        public HostService(UnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public OperationStatus Create(HostDTO hostDto, string role)
        {
            Host host = Database.HostRepository.GetOne(user => user.Email == hostDto.Email);
            if (host == null)
            {
                host = new Host { Email = hostDto.Email, Name = hostDto.Email, Role = role, Password = hostDto.Password };
                Database.HostRepository.Create(host);
                Database.Save();
                return new OperationStatus(true, "Registration was sucssesful", "");
            }
            else
            {
                return new OperationStatus(false, "User with this name alredy exsits", "Email");
            }
        }

        public ClaimsIdentity Authenticate(HostDTO hostDto)
        {
            ClaimsIdentity claim = null;
            Host host = Database.HostRepository.GetOne(user => user.Email == hostDto.Email && user.Password == hostDto.Password);
            if (host != null && !host.IsBlocked)
            {
                claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, host.Id.ToString(), ClaimValueTypes.String));
                claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, host.Email, ClaimValueTypes.String));
                claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                    "OWIN Provider", ClaimValueTypes.String));
                host.TryingsToEnter = 0;
                Database.HostRepository.Update(host);
            }
            else
            {
                host = Database.HostRepository.GetOne(user => user.Email == hostDto.Email);
                host.TryingsToEnter++;
                if(host.TryingsToEnter > 4)
                {
                    host.IsBlocked = true;
                    Database.HostRepository.Update(host);
                }
                Database.HostRepository.Update(host);
            }
            return claim;
        }

        public OperationStatus Edit(HostDTO host)
        {
            if (host != null)
            {
                Database.HostRepository.Update(HostDTO.ToHost(host));
                Database.Save();
                return new OperationStatus(true, "Changes were succsesfull", "");
            }
            else
            {
                return new OperationStatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public void Dispose()
        {
            Database.Dispose();
        }

        public OperationStatus UnlockUser(string email)
        {
            Host hostEdited = Database.HostRepository.GetOne(host => host.Email == email);
            hostEdited.IsBlocked = false;
            if (hostEdited != null)
            {
                Database.HostRepository.Update(hostEdited);
                return new OperationStatus(true, "Changes were succsesfull", "");
            }
            else
            {
                return new OperationStatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public HostDTO Get(string email)
        {
            return HostDTO.ToHostDTO(Database.HostRepository.GetOne(host => host.Email == email));
        }

        public IEnumerable<HostDTO> GetAll()
        {
            return Database.HostRepository.GetAll()
                .Select(host => HostDTO.ToHostDTO(host));
        }

        public HostDTO GetById(int? id)
        {
            HostDTO hostDTO =  HostDTO.ToHostDTO(Database.HostRepository.GetById(id));
            hostDTO.Magazines = Database.MagazineRepository
                .Get(magazine => magazine.Hosts.Contains(Database.HostRepository.GetById(id)))
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine))
                .ToList();
            return hostDTO;
        }
    }
}
