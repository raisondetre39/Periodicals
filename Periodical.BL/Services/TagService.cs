using Periodical.BL.DataTemporaryModels;
using Periodical.BL.ServiseInterfaces;
using Periodicals.DAL.Publishings;
using Periodicals.DAL.UnitOfWork;
using System.Collections.Generic;
using System.Linq;

namespace Periodical.BL.Services
{
    public class TagService : ITagService
    {
        private readonly log4net.ILog log = log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        IUnitOfWork Database { get; set; }

        public TagService()
        {
            Database = new UnitOfWork("DefaultConnection");
        }

        public TagService(IUnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public List<Tag> GetAll()
        {
            log.Info("Sent request to data base to get all tags");
            return Database.TagRepository.GetAll().ToList();
        }

        public Tag Get(string name)
        {
            log.Info($"Sent request to data base to get tag with name: {name}");
            return Database.TagRepository.GetOne(tag => tag.TagName == name);
        }

        public Tag GetById(int? id)
        {
            log.Info($"Sent request to data base to get tag by id: {id}");
            return Database.TagRepository.GetById(id);
        }

        public List<MagazineDTO> GetByTagName(string name)
        {
            log.Info($"Sent request to data base to get magazine contains tag with name: {name}");
            List<Magazine> magazinesContainsCurrentTag = Database.MagazineRepository
                .Get(magazine => magazine.Tags.Contains(Database.TagRepository.GetOne(tag => tag.TagName == name)))
                .ToList();
            if (magazinesContainsCurrentTag.Count > 0)
                return magazinesContainsCurrentTag
                    .Select(magazine => MagazineDTO.ToMagazineDTO(magazine))
                    .ToList();
            return new List<MagazineDTO>();
        }
    }
}
