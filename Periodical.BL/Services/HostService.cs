using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using System.Security.Claims;
using Periodicals.DAL.UnitOfWork;
using Periodicals.DAL.Accounts;
using System.Collections.Generic;
using System.Linq;

namespace Periodical.BL.Services
{

    /// <summary>
    /// Class creates service to magage all users operations in presentation layer
    /// </summary>
    public class HostService : IHostService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IUnitOfWork Database { get; set; }

        public HostService()
        {
            Database = new UnitOfWork();
        }

        public HostService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public OperationStatus Create(HostDTO hostDto, string role)
        {
            Host host = Database.HostRepository.GetOne(user => user.Email == hostDto.Email);
            log.Debug($"User is trying to create {role} profile");
            if (host == null)
            {
                host = new Host { Email = hostDto.Email, Name = hostDto.Email, Role = role, Password = hostDto.Password };
                Database.HostRepository.Create(host);
                Database.Save();
                log.Info($"User with email: {host.Email} created profile succsesfuly");
                return new OperationStatus(true, "Registration was sucssesful", "");
            }
            else
            {
                log.Info($"User with email: {host.Email} denied to create profile");
                return new OperationStatus(false, "User with this name alredy exsits", "Email");
            }
        }

        /// <summary>
        /// Methods creates claims base on host id and email
        /// </summary>
        /// <param name="hostDto"></param>
        /// <returns>Claims</returns>
        public ClaimsIdentity Authenticate(HostDTO hostDto)
        {
            ClaimsIdentity claim = null;
            Host host = Database.HostRepository.GetOne(user => user.Email == hostDto.Email && user.Password == hostDto.Password);
            log.Debug($"User with email: {hostDto.Email} is trying to authenticate to resource");
            if (host != null && !host.IsBlocked)
            {
                claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, host.Id.ToString(), ClaimValueTypes.String));
                claim.AddClaim(new Claim(ClaimTypes.Role, host.Role.ToString(), ClaimValueTypes.String));
                claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, host.Email, ClaimValueTypes.String));
                claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                    "OWIN Provider", ClaimValueTypes.String));
                host.TryingsToEnter = 0;
                Database.HostRepository.Update(host);
                log.Info($"User with id: {host.Id} got claims succsesfuly");
            }
            else
            {
                host = Database.HostRepository.GetOne(user => user.Email == hostDto.Email);
                host.TryingsToEnter++;
                if(host.TryingsToEnter > 4)
                {
                    host.IsBlocked = true;
                    Database.HostRepository.Update(host);
                    log.Info($"User with id: {host.Id} is blocked");
                }
                Database.HostRepository.Update(host);
                log.Info($"User with id: {host.Id} denied to get claims");
            }
            return claim;
        }

        public OperationStatus Edit(HostDTO host)
        {
            log.Debug($"Host is trying to edit profile");
            if (host != null)
            {
                Host hostEdited = HostDTO.ToHost(host);
                hostEdited.Password = host.Password;
                hostEdited.Name = host.Name;
                hostEdited.Email = host.Email;
                Database.HostRepository.Update(hostEdited);
                log.Info($"Host with id: {host.Id} update profile succsesfull");
                return new OperationStatus(true, "Changes were succsesfull", "");
            }
            else
            {
                log.Warn($"Host denied to update profile");
                return new OperationStatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public OperationStatus EditUserWallet(int id, int sum)
        {
            log.Debug($"Host is trying to edit profile");
            Host host = Database.HostRepository.GetById(id);
            if (host != null)
            {
                host.Wallet += sum;
                Database.HostRepository.Update(host);

                log.Info($"Host with id: {host.Id} update profile succsesfull");
                return new OperationStatus(true, "Changes were succsesfull", "");
            }
            else
            {
                log.Warn($"Host denied to update profile");
                return new OperationStatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public HostDTO Get(string email)
        {
            log.Info($"Sent request to data base to get host with email: {email}");
            Host currentHost = Database.HostRepository.GetOne(host => host.Email == email);
            if(currentHost != null)
                return HostDTO.ToHostDTO(currentHost);
            return null;
        }

        public List<HostDTO> GetAll()
        {
            log.Info($"Sent request to data base to get all hosts");
            return Database.HostRepository.GetAll()
                .Select(host => HostDTO.ToHostDTO(host))
                .ToList();
        }

        public HostDTO GetById(int? id)
        {
            log.Info($"Sent request to data base to get host with id: {id}");
            var entity = Database.HostRepository.GetById(id);
            HostDTO hostDTO =  HostDTO.ToHostDTO(entity);
            hostDTO.Magazines = entity
                .Magazines
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine))
                .ToList();
            return hostDTO;
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
