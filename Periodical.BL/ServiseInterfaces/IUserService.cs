using Periodical.BL.DataTemporaryModels;
using Periodical.BL.Infrastructure;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Periodical.BL.Services
{
    // Interface implements cooperation between Presentation layer and Data Access layer
    public interface IUserService : IDisposable 
    {
        OperationSatus CreateUser(HostDTO userDTO, string role);
        OperationSatus EditUser(HostDTO userDTO);
        OperationSatus AddUserMagazine(HostDTO userDTO, int magasineId);
        OperationSatus DeleteUserMagazine(HostDTO userDTO, int magasineId);
        ClaimsIdentity Authenticate(HostDTO userDTO);
        OperationSatus CreateMagasine(MagazineDTO magazineDTO, int id);
        OperationSatus EditMagazine(MagazineDTO magazineDTO);
        OperationSatus BlockUser(string email);
        OperationSatus UnlockUser(string email);
    }
}
