using Periodical.BL.Services;

namespace Periodical.BL.ServiseInterfaces
{
    public interface IServiceCreator
    {
        HostService CreateHostService(string connection);
    }
}
