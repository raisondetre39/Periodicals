using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using System.Collections.Generic;

namespace Periodical.BL.ServiseInterfaces
{
    public interface IHostMagazineService
    {
        OperationStatus Create(HostDTO userDTO, int magasineId);

        OperationStatus Delete(HostDTO userDTO, int? magasineId);

        List<MagazineDTO> GetUserMagazines(int id);
    }
}
