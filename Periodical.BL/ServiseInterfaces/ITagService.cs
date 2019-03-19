using Periodical.BL.DataTemporaryModels;
using Periodicals.DAL.Publishings;
using System.Collections.Generic;

namespace Periodical.BL.ServiseInterfaces
{
    public interface ITagService
    {
        IEnumerable<Tag> GetAll();

        Tag Get(string name);

        Tag GetById(int? id);

        IEnumerable<MagazineDTO> GetByTagName(string name);
    }
}
