using Periodicals.DAL.Publishings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Periodical.BL.DataTemporaryModels
{
    public class MagazineDTO
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string MagazineName { get; set; }
        public DateTime PublishDate { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Range(1, 1000,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int Price { get; set; }
        public string Cover { get; set; }
        public virtual List<HostDTO> Hosts { get; set; }
        public virtual List<Tag> Tags { get; set; }
        public int HostId { get; set; }

        public static Magazine ToMagazine(MagazineDTO magazineDTO, int authorId)
        {
            Magazine magazine = new Magazine()
            {
                MagazineName = magazineDTO.MagazineName,
                HostId = authorId,
                Cover = magazineDTO.Cover,
                Description = magazineDTO.Description,
                Tags = magazineDTO.Tags,
                Price = magazineDTO.Price
            };
            return magazine;
        }

        public static Magazine ToMagazine(MagazineDTO magazineDTO)
        {
            Magazine magazine = new Magazine()
            {
                MagazineName = magazineDTO.MagazineName,
                HostId = magazineDTO.HostId,
                Cover = magazineDTO.Cover,
                Description = magazineDTO.Description,
                Tags = magazineDTO.Tags,
                Price = magazineDTO.Price
            };
            return magazine;
        }

        public static MagazineDTO ToMagazineDTO(Magazine magazine)
        {
            MagazineDTO magazineDTO = new MagazineDTO()
            {
                Id = magazine.MagazineId,
                Cover = magazine.Cover,
                PublishDate = magazine.PublishDate,
                Description = magazine.Description,
                Tags = magazine.Tags,
                Price = magazine.Price,
                MagazineName = magazine.MagazineName
            };
            return magazineDTO;
        }
    }
}
