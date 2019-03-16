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
    public class MagazineService : IMagazineService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IUnitOfWork Database { get; set; }

        public MagazineService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public OperationStatus Create(MagazineDTO magazineDTO, HostDTO author)
        {
            Magazine magazine = MagazineDTO.ToMagazine(magazineDTO, author.Id);
            log.Debug($"Auhtor with id: {author.Id} is trying to create magazine: {magazineDTO.MagazineName}");
            if (magazine != null)
            {
                Database.MagazineRepository.Create(magazine);
                Host authorEdited = Database.HostRepository.GetById(author.Id);
                authorEdited.Magazines.Add(magazine);
                log.Info("Magazine created succsesfully");
                return new OperationStatus(true, "Create was succsesfull", "");
            }
            else
            {
                log.Warn($"Auhtor with id: {author.Id} denied to create magazine: {magazineDTO.MagazineName}");
                return new OperationStatus(false, "Something went wrong", "MagazineCreate");
            }
        }

        public OperationStatus Edit(MagazineDTO magazineDTO)
        {
            Magazine magazineEdited = MagazineDTO.ToMagazine(magazineDTO, magazineDTO.HostId);
            log.Debug($"Auhtor with id: {magazineDTO.HostId} is trying to edit magazine: {magazineDTO.Id}");
            if (magazineEdited != null)
            {
                Database.MagazineRepository.Update(magazineEdited);
                log.Info($"Magazine with id: {magazineDTO.Id} updated succsesfully");
                return new OperationStatus(true, "Update was succsesfull", "");
            }
            else
            {
                log.Warn($"Auhtor with id: {magazineDTO.HostId} denied to update magazine: {magazineDTO.MagazineName}");
                return new OperationStatus(false, "Something went wrong", "MagazineUpdate");
            }
        }

        public MagazineDTO GetById(int? id)
        {
            log.Info($"Sent request to data base to get magazine with id: {id}");
            Magazine magazine = Database.MagazineRepository.GetById(id);
            MagazineDTO magazineDTO = MagazineDTO.ToMagazineDTO(magazine);
            return magazineDTO;
        }

        public MagazineDTO Get(string name)
        {
            Magazine magazineCurrent = Database.MagazineRepository.GetOne(magazine => magazine.MagazineName == name);
            log.Debug($"Sent request to data base to magazine with name: {name}");
            if (magazineCurrent != null)
            {
                MagazineDTO magazineDTO = MagazineDTO.ToMagazineDTO(magazineCurrent);
                log.Info($"Magazine with name: {name} find succsesfuly");
                return magazineDTO;
            }
            return null;
        }

        public IEnumerable<MagazineDTO> GetAll()
        {
            log.Info("Sent request to data base to get all magazines");
            return Database.MagazineRepository.GetAll()
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine));
        }

        public IEnumerable<MagazineDTO> GetBy(string name)
        {
            log.Info($"Sent request to data base to get all magazines with name: {name}");
            return Database.MagazineRepository.Get(magazine => magazine.MagazineName == name)
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine));
        }

        public OperationStatus Delete(int? id)
        {
            log.Info($"Sent request to data base to delete magazine with id: {id}");
            Database.MagazineRepository.Delete(id);
            return new OperationStatus(true, "Delete was succsesfull", "");
        }
    }
}
