using Periodicals.DAL.Accounts;

namespace Periodicals.DAL.Publishings
{
    public class HostMagazine
    {
        public int HostMagazineId { get; set; }

        public int? HostId { get; set; }

        public int? MagazineId { get; set; }

        public virtual Host Host { get; set; }

        public virtual Magazine Magazine { get; set; }
    }
}
