using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using System.Collections.Generic;

namespace Periodical.BL.ServiseInterfaces
{
    public interface IAdminService
    {
        OperationStatus UnlockUser(int? id);

        OperationStatus BlockUser(int? id);

        List<HostDTO> GetBlockedUsers();

        List<HostDTO> GetUnlockedUsers();
    }
}
