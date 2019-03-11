using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using System.Collections.Generic;

namespace Periodical.BL.ServiseInterfaces
{
    public interface IMagazineService
    {
        OperationStatus Create(MagazineDTO magazineDTO, HostDTO author);

        OperationStatus Edit(MagazineDTO magazineDTO);

        MagazineDTO GetById(int? id);

        MagazineDTO Get(string name);

        IEnumerable<MagazineDTO> GetAll();

        IEnumerable<MagazineDTO> GetBy(string name);

        OperationStatus Delete(int? id);

        void Dispose();
    }
}
