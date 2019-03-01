using Periodicals.DAL.Publishings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Periodicals.DAL.Accounts
{
    public class Host
    {
        [Required]
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ProfilePicture { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get { return false; } set { } }
        [DataType(DataType.Currency)]
        public int Vallet { get; set; }
        public List<Magazine> Magazines { get; set; }
        public Host()
        {
            Magazines = new List<Magazine>();
        }
    }
}
