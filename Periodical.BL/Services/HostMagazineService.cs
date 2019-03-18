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

    /// <summary>
    /// Class creates service with methods, which manage operations with user's magazine list and magazines hosts list
    /// </summary>
    public class HostMagazineService : IHostMagazineService
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IUnitOfWork Database { get; set; }

        public HostMagazineService()
        {
            Database = new UnitOfWork(); 
        }

        public HostMagazineService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }


        public OperationStatus AddMagazine(HostDTO userDTO, int magasineId)
        {
            Host hostEdited = Database.HostRepository.GetById(userDTO.Id);
            Magazine magazineToAdd = Database.MagazineRepository.GetById(magasineId);
            log.Info($"User is trying to add to own list magazine");
            if (hostEdited != null && magazineToAdd != null)
            {
                hostEdited.Wallet -= magazineToAdd.Price;
                hostEdited.Magazines.Add(magazineToAdd);
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
            Magazine magazine = Database.MagazineRepository.GetById(magasineId); 
            log.Info($"User is trying to delete from own list magazine");
            if (hostEdited != null && magazine != null)
            {
                hostEdited.Magazines.Remove(Database.MagazineRepository.GetById(magasineId));
                magazine.Hosts.Remove(hostEdited);
                Database.HostRepository.Update(hostEdited);
                Database.MagazineRepository.Update(magazine);
                log.Info($"Magazine with id {magasineId} deleted succsesfully by user with id: {userDTO.Id} ");
                return new OperationStatus(true, "Changes were succsesfull", "");
            }
            else
            {
                log.Info($"User is denied to delete magazine to list");
                return new OperationStatus(false, "Something went wrong", "ProfileChange");
            }
        }

        public List<MagazineDTO> GetUserMagazines(int id)
        {
            log.Info($"Get all user`s ( id: {id} ) magazines");
            return Database.HostRepository.GetById(id)
                .Magazines
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine))
                .ToList();
        }
    }
}
