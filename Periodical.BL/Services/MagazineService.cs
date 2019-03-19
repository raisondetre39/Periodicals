using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using Periodical.BL.ServiseInterfaces;
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
    public class MagazineService : IMagazineService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IUnitOfWork Database { get; set; }

        public MagazineService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public OperationStatus Create(MagazineDTO magazineDTO, int authorId, int[] tags)
        {
            Magazine magazineCurrent = Database.MagazineRepository.GetOne(magazine => magazine.MagazineName == magazineDTO.MagazineName);
            log.Debug($"Author is trying to create magazine");
            if (magazineCurrent == null)
            {
                var magazine = MagazineDTO.ToMagazine(magazineDTO);
                magazine.HostId = magazineDTO.HostId;
                magazine.Tags = new List<Tag>();
                magazine.PublishDate = DateTime.Now;
                Database.MagazineRepository.Create(magazine);
                var author = Database.HostRepository.GetById(authorId);
                magazineCurrent = Database.MagazineRepository
                    .GetOne(createdMagazine => createdMagazine.MagazineName == magazine.MagazineName);
                author.Magazines.Add(magazineCurrent);
                Database.HostRepository.Update(author);
                if (tags != null)
                {
                    for (int i = 0; i < tags.Length; i++)
                    {
                        var tempTag = Database.TagRepository.GetById(tags[i]);
                        tempTag.Magazines.Add(magazineCurrent);
                        Database.TagRepository.Update(tempTag);
                    }
                }
                log.Info($"Magazine with name: {magazineDTO.MagazineName} created succsesfully by auhtor with id: {authorId} ");
                return new OperationStatus(true, "Create was succsesfull", "");
            }
            else
            {
                log.Warn($"Auhtor denied to create magazine");
                return new OperationStatus(false, "Something went wrong", "MagazineCreate");
            }
        }

        public OperationStatus Edit(MagazineDTO magazineDTO, int authorId, int[] tags)
        {
            log.Debug($"Author is trying to edit magazine");
            if (magazineDTO != null)
            {
                var magazineEdited = MagazineDTO.ToMagazine(magazineDTO);
                magazineEdited.Tags = new List<Tag>();
                magazineEdited.PublishDate = DateTime.Now;
                magazineEdited.HostId = authorId;
                Database.MagazineRepository.Update(magazineEdited);
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
    }
}
