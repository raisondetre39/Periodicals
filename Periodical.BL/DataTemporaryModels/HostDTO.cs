using Periodicals.DAL.Accounts;
using System.Collections.Generic;
using System.Linq;

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
        public int Wallet { get; set; }
        public bool IsBlocked { get { return false; } set { } }
        public virtual List<MagazineDTO> Magazines { get; set; }

        public HostDTO()
        {
            Magazines = new List<MagazineDTO>();
        }

        public static Host ToHost(HostDTO hostDTO)
        {
            Host host = new Host();
            host.Id = hostDTO.Id;
            host.Role = hostDTO.Role;
            host.Name = hostDTO.Name;
            host.Email = hostDTO.Email;
            host.Password = hostDTO.Password;
            host.ProfilePicture = hostDTO.ProfilePicture;
            host.Wallet = hostDTO.Wallet;
            host.Magazines = hostDTO.Magazines.Select(magazine => MagazineDTO.ToMagazine(magazine)).ToList();
            return host;
        }

        public static HostDTO ToHostDTO(Host host)
        {
            HostDTO hostDTO = new HostDTO();
            hostDTO.Id = host.Id;
            hostDTO.Name = host.Name;
            hostDTO.Role = host.Role;
            hostDTO.Password = host.Password;
            hostDTO.Email = host.Email;
            hostDTO.ProfilePicture = host.ProfilePicture;
            hostDTO.Wallet = host.Wallet;
            hostDTO.Magazines = host.Magazines.Select(magazine => MagazineDTO.ToMagazineDTO(magazine)).ToList();
            return hostDTO;
        }
    }
}
