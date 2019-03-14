using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using Periodical.BL.ServiseInterfaces;
using Periodicals.DAL.Accounts;
using Periodicals.DAL.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace Periodical.BL.Services
{
    public class AdminService : IAdminService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        UnitOfWork Database { get; set; }

        public AdminService()
        {
            Database = new UnitOfWork("DefaultConnection");
        }

        public OperationStatus UnlockUser(int? id)
        {
            Host hostEdited = Database.HostRepository.GetById(id);
            hostEdited.IsBlocked = false;
            log.Debug($"Check if user with id: {id} became unlocked");
            if (hostEdited != null)
            {
                Database.HostRepository.Update(hostEdited);
                log.Info($"User with id: {id} succsesfully unlocked");
                return new OperationStatus(true, "Changes were succsesfull", "");
            }
            else
            {
                log.Warn($"User with id: {id} denied to unlock");
                return new OperationStatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public OperationStatus BlockUser(int? id)
        {
            Host hostEdited = Database.HostRepository.GetById(id);
            hostEdited.IsBlocked = true;
            log.Debug($"Check if user with id: {id} became blocked");
            if (hostEdited != null)
            {
                Database.HostRepository.Update(hostEdited);
                log.Info($"User with id: {id} succsesfully blocked");
                return new OperationStatus(true, "Changes were succsesfull", "");
            }
            else
            {
                log.Warn($"User with id: {id} denied to block");
                return new OperationStatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public List<HostDTO> GetBlockedUsers()
        {
            log.Info("Requet to data base to get blocked users");
            return Database.HostRepository.Get(host => host.IsBlocked)
                .Select(host => HostDTO.ToHostDTO(host))
                .ToList();
        }

        public List<HostDTO> GetUnlockedUsers()
        {
            log.Info("Requet to data base to get unlocked users");
            return Database.HostRepository.Get(host => !host.IsBlocked && host.Email != "admin@gmail.com")
                .Select(host => HostDTO.ToHostDTO(host))
                .ToList();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
