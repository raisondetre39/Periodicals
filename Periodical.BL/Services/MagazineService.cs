﻿using Periodical.BL.DataTemporaryModels;
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
            Magazine magazine = Database.MagazineRepository.GetOne(magazineCurrent => magazineCurrent.MagazineName == magazineDTO.MagazineName);
            log.Debug($"Author is trying to create magazine");
            if (magazine == null)
            {
                magazine = MagazineDTO.ToMagazine(magazineDTO);
                magazine.HostId = magazineDTO.HostId;
                magazine.Tags = new List<Tag>();
                magazine.PublishDate = DateTime.Now;
                if (tags != null)
                {
                    foreach(var tag in tags.Select(tagId => Database.TagRepository.GetById(tagId)).ToList())
                    {
                        tag.TagMagazine = null;
                        tag.Magazines.Add(magazine);
                        Database.TagRepository.Update(tag);
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
                Database.MagazineRepository.Delete(magazineEdited.MagazineId);
                if (tags != null)
                {
                    foreach (var tag in tags.Select(tagId => Database.TagRepository.GetById(tagId)).ToList())
                    {
                        tag.TagMagazine = null;
                        tag.Magazines.Add(magazineEdited);
                        Database.TagRepository.Update(tag);
                    }
                }
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

        public IEnumerable<MagazineDTO> GetAuthorMagazines(int authorId)
        {
            return Database.MagazineRepository
                .Get(magazine => magazine.HostId == authorId)
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine))
                .ToList();
        }
    }
}
