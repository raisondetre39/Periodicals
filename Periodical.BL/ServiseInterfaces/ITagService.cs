using Periodical.BL.DataTemporaryModels;
using Periodicals.DAL.Publishings;
using System.Collections.Generic;

namespace Periodical.BL.ServiseInterfaces
{
    public interface ITagService
    {
        List<Tag> GetAll();

        Tag Get(string name);

        Tag GetById(int? id);

        List<MagazineDTO> GetByTagName(string name);
    }
}
