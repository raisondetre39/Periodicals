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
        private readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IUnitOfWork Database { get; set; }

        public HostMagazineService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public OperationStatus Create(HostDTO userDTO, int magasineId)
        {
            Host hostEdited = Database.HostRepository.GetById(userDTO.Id);
            log.Info($"User is trying to add to own list magazine");
            if (hostEdited != null)
            {
                hostEdited.Magazines.Add(Database.MagazineRepository.GetById(magasineId));
                Database.HostRepository.Update(hostEdited);
                log.Info($"Magazine with id: {magasineId} added succsesfully to user with id: {userDTO.Id}");
                return new OperationStatus(true, "Changes were succsesfull", "");
            }
            else
            {
                log.Info($"User is denied to add magazine to list");
                return new OperationStatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public OperationStatus Delete(HostDTO userDTO, int? magasineId)
        {
            Host hostEdited = Database.HostRepository.GetById(userDTO.Id);
            hostEdited.Magazines.Remove(Database.MagazineRepository.GetById(magasineId));
            Magazine magazine = Database.MagazineRepository.GetById(magasineId);
            magazine.Hosts.Remove(hostEdited);
            log.Info($"User with id: {userDTO.Id} is trying to delete from own list magazine with id {magasineId}");
            if (hostEdited != null)
            {
                Database.HostRepository.Update(hostEdited);
                Database.MagazineRepository.Update(magazine);
                log.Info("Magazine deleted succsesfully");
                return new OperationStatus(true, "Changes were succsesfull", "");
            }
            else
            {
                log.Info($"User is denied to delete magazine with id: {magasineId} to list");
                return new OperationStatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public List<MagazineDTO> GetUserMagazines(int id)
        {
            log.Info($"Get all user`s ( id: {id} ) magazines");
            return Database.MagazineRepository
                .Get(magazine => magazine.Hosts.Contains(Database.HostRepository.GetById(id)))
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine))
                .ToList();
        }
    }
}
