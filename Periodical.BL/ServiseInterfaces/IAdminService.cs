using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Periodical.BL.ServiseInterfaces
{
    public interface IAdminService
    {
        OperationStatus UnlockUser(int? id);

        OperationStatus BlockUser(int? id);

        List<HostDTO> BlockedUsers();

        List<HostDTO> UnlockedUsers();

        void Dispose();
    }
}
