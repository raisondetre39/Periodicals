using System.Collections.Generic;

namespace Periodical.BL.DataTemporaryModels
{
    public class HostDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string ProfilePicture { get; set; }
        public string Role { get; set; }
        public int Vallet { get; set; }
        public bool IsBlocked { get { return false; } set { } }
        public List<MagazineDTO> Magazines { get; set; }
    }
}
