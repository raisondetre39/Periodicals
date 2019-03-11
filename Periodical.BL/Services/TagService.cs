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
        UnitOfWork Database { get; set; }

        public TagService(UnitOfWork unitOfWork)
        {
            Database = unitOfWork;
        }

        public IEnumerable<Tag> GetAll()
        {
            return Database.TagRepository.GetAll();
        }

        public Tag Get(string name)
        {
            return Database.TagRepository.GetOne(tag => tag.TagName == name);
        }

        public Tag GetById(int? id)
        {
            return Database.TagRepository.GetById(id);
        }

        public List<MagazineDTO> GetByTagName(string name)
        {
            return Database.TagRepository.GetOne(tag => tag.TagName == name)
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
