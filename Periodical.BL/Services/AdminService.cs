using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using Periodical.BL.ServiseInterfaces;
using Periodicals.DAL.Accounts;
using Periodicals.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Periodical.BL.Services
{
    public class AdminService : IAdminService
    {
        UnitOfWork Database { get; set; }

        public AdminService()
        {
            Database = new UnitOfWork("DefaultConnection");
        }

        public OperationStatus UnlockUser(int? id)
        {
            Host hostEdited = Database.HostRepository.GetById(id);
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

        public OperationStatus BlockUser(int? id)
        {
            Host hostEdited = Database.HostRepository.GetById(id);
            hostEdited.IsBlocked = true;
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

        public List<HostDTO> BlockedUsers()
        {
            return Database.HostRepository.Get(host => host.IsBlocked)
                .Select(host => HostDTO.ToHostDTO(host))
                .ToList();
        }

        public List<HostDTO> UnlockedUsers()
        {
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
