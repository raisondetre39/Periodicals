using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using Periodical.BL.ServiseInterfaces;
using Periodicals.DAL.Accounts;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Periodical.BL.Services
{
    /// <summary>
    /// Class creates service to manage all operations with magazines
    /// </summary>
    public class MagazineService : IMagazineService, IDisposable
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IUnitOfWork Database { get; set; }

        public MagazineService()
        {
            Database = new UnitOfWork();
        }

        public MagazineService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public OperationStatus Create(MagazineDTO magazineDTO, HostDTO author)
        {
            Magazine magazineCurrent = Database.MagazineRepository.GetOne(magazine => magazine.MagazineName == magazineDTO.MagazineName);
            log.Debug($"Author is trying to create magazine");
            if (magazineCurrent == null)
            {
                //Host authorEdited = Database.HostRepository.GetById(author.Id);
                //authorEdited.Magazines.Add(MagazineDTO.ToMagazine(magazineDTO));
                //Database.HostRepository.Update(authorEdited);
                var magazine = MagazineDTO.ToMagazine(magazineDTO, author.Id);
                Database.MagazineRepository.Create(magazine);
                log.Info($"Magazine with name: {magazineDTO.MagazineName} created succsesfully by auhtor with id: {author.Id} ");
                return new OperationStatus(true, "Create was succsesfull", "");
            }
            else
            {
                log.Warn($"Auhtor denied to create magazine");
                return new OperationStatus(false, "Something went wrong", "MagazineCreate");
            }
        }

        public OperationStatus Edit(MagazineDTO magazineDTO, int authorId)
        {
            log.Debug($"Author is trying to edit magazine");
            if (magazineDTO != null)
            {
                Database.MagazineRepository.Update(MagazineDTO.ToMagazine(magazineDTO, authorId));
                log.Info($"Magazine with id: {magazineDTO.Id} updated succsesfully by auhtor with id: {magazineDTO.HostId}");
                return new OperationStatus(true, "Update was succsesfull", "");
            }
            else
            {
                log.Warn($"Auhtor denied to update magazine");
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

        public IEnumerable<MagazineDTO> GetAll()
        {
            log.Info("Sent request to data base to get all magazines");
            return Database.MagazineRepository.GetAll()
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine))
                .ToList();
        }

        public IEnumerable<MagazineDTO> GetBy(string name)
        {
            log.Info($"Sent request to data base to get all magazines with name: {name}");
            return Database.MagazineRepository.Get(magazine => magazine.MagazineName == name)
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine))
                .ToList();
        }

        public OperationStatus Delete(int? id)
        {
            log.Info($"Sent request to data base to delete magazine with id: {id}");
            Database.MagazineRepository.Delete(id);
            return new OperationStatus(true, "Delete was succsesfull", "");
        }

        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
