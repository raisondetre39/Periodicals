using Periodicals.DAL.Accounts;

namespace Periodicals.DAL.Publishings
{
    /// <summary>
    ///  Class creates coonection between magazines list in host class and hosts list in magazines class
    /// </summary>
    public class HostMagazine
    {
        public int HostMagazineId { get; set; }

        public int? HostId { get; set; }

        public int? MagazineId { get; set; }

        public virtual Host Host { get; set; }

        public virtual Magazine Magazine { get; set; }
    }
}
