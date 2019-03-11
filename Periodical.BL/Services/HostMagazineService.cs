using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using Periodical.BL.ServiseInterfaces;
using Periodicals.DAL.Accounts;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace Periodical.BL.Services
{
    public class HostMagazineService : IHostMagazineService
    {
        UnitOfWork Database { get; set; }

        public HostMagazineService(UnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public OperationStatus Create(HostDTO userDTO, int magasineId)
        {
            Host hostEdited = Database.HostRepository.GetById(userDTO.Id);
            hostEdited.Magazines.Add(Database.MagazineRepository.GetById(magasineId));
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

        public OperationStatus Delete(HostDTO userDTO, int? magasineId)
        {
            Host hostEdited = Database.HostRepository.GetById(userDTO.Id);
            hostEdited.Magazines.Remove(Database.MagazineRepository.GetById(magasineId));
            Magazine magazine = Database.MagazineRepository.GetById(magasineId);
            magazine.Hosts.Remove(hostEdited);
            if (hostEdited != null)
            {
                Database.HostRepository.Update(hostEdited);
                Database.MagazineRepository.Update(magazine);
                return new OperationStatus(true, "Changes were succsesfull", "");
            }
            else
            {
                return new OperationStatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public List<MagazineDTO> GetUserMagazines(int id)
        {
            return Database.HostRepository.GetById(id)
                .Magazines
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine))
                .ToList();
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
