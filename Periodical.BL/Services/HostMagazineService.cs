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

        private IUnitOfWork Database { get; set; }

        public HostMagazineService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public OperationStatus AddMagazine(HostDTO userDTO, int? magasineId)
        {
            Host hostEdited = Database.HostRepository.GetById(userDTO.Id);
            Magazine magazineToAdd = Database.MagazineRepository.GetById(magasineId);
            log.Info($"User is trying to add to own list magazine");
            if (userDTO != null && magazineToAdd != null)
            {
                hostEdited.Wallet -= magazineToAdd.Price;
                hostEdited.HostMagazine = null;
                Database.HostRepository.Update(hostEdited);
                Host hostToAdd = hostEdited;
                hostToAdd.Id = userDTO.Id;
                magazineToAdd.Hosts.Add(hostToAdd);
                Database.MagazineRepository.Update(magazineToAdd);
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
                magazine.Hosts.Remove(magazine.Hosts.Single(userToFind => userToFind.Email == hostEdited.Email));
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

        public IEnumerable<MagazineDTO> GetUserMagazines(int id)
        {
            log.Info($"Get all user`s ( id: {id} ) magazines");
            List<Magazine> userList = new List<Magazine>();
            foreach(var mag in Database.MagazineRepository.GetAll())
            {
                if(mag.Hosts.Any(host => host.Email == Database.HostRepository.GetById(id).Email))
                {
                    userList.Add(mag);
                }
            }
            return userList.Select(magazine => MagazineDTO.ToMagazineDTO(magazine));
        }
    }
}
