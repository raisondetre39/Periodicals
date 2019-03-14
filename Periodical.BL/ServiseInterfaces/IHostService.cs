using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using System.Collections.Generic;
using System.Security.Claims;

namespace Periodical.BL.Services
{
    public interface IHostService
    {
        OperationStatus Create(HostDTO hostDto, string role);

        ClaimsIdentity Authenticate(HostDTO hostDto);

        OperationStatus Edit(HostDTO host);

        HostDTO Get(string email);

        IEnumerable<HostDTO> GetAll();

        HostDTO GetById(int? id);

        void Dispose();
    }
}
