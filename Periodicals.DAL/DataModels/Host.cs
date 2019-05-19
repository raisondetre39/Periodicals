using Periodicals.DAL.Publishings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Periodicals.DAL.Accounts
{
    /// <summary>
    ///  Class creates fields for host's accounts
    /// </summary>
    public class Host
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ProfilePicture { get; set; }

        public string Role { get; set; }

        public int TryingsToEnter { get; set; }

        public bool IsBlocked { get; set; }

        [DataType(DataType.Currency)]
        public int Wallet { get; set; }

        public virtual List<Magazine> Magazines { get; set; }

        public virtual List<HostMagazine> HostMagazine { get; set; }

        public Host()
        {
            HostMagazine = new List<HostMagazine>();
            Magazines = new List<Magazine>();
        }
    }
}
