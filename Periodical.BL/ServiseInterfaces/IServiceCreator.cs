using Periodical.BL.Services;

namespace Periodical.BL.ServiseInterfaces
{
    interface IServiceCreator
    {
        IService CreateService(string connection);
    }
}
