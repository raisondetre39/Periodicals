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
            Host host = new Host
            {
                Id = hostDTO.Id,
                Role = hostDTO.Role,
                Name = hostDTO.Name,
                Email = hostDTO.Email,
                Password = hostDTO.Password,
                ProfilePicture = hostDTO.ProfilePicture,
                Wallet = hostDTO.Wallet,
                Magazines = hostDTO.Magazines
                .Select(magazine => MagazineDTO.ToMagazine(magazine))
                .ToList()
            };
            return host;
        }

        public static HostDTO ToHostDTO(Host host)
        {
            HostDTO hostDTO = new HostDTO
            {
                Id = host.Id,
                Name = host.Name,
                Role = host.Role,
                Password = host.Password,
                Email = host.Email,
                ProfilePicture = host.ProfilePicture,
                Wallet = host.Wallet,
                Magazines = host.Magazines
                .Select(magazine => MagazineDTO.ToMagazineDTO(magazine))
                .ToList()
            };
            return hostDTO;
        }
    }
}
