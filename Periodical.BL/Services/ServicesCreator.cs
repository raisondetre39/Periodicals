using Periodical.BL.ServiseInterfaces;
using Periodicals.DAL.UnitOfWork;

namespace Periodical.BL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public HostService CreateHostService(string connection)
        {
            return new HostService(new UnitOfWork(connection));
        }
    }
}
