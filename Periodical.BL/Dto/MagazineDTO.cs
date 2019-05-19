using Periodicals.DAL.Publishings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Periodical.BL.DataTemporaryModels
{
    /// <summary>
    /// Class creates Magazine temporary model to work woth data got from presentation layer
    /// </summary>
    public class MagazineDTO
    {
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name can't be empty")]
        public string MagazineName { get; set; }

        public DateTime PublishDate { get; set; }

        [Required(ErrorMessage = "Description can't be empty")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Range(1, 1000,
        ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        [Required(ErrorMessage = "Price can't be empty")]
        public int Price { get; set; }

        public string Cover { get; set; }

        public virtual List<HostDTO> Hosts { get; set; }
        
        public virtual List<Tag> Tags { get; set; }

        public int HostId { get; set; }

        /// <summary>
        /// Method make covertation from MagazineDTO to Magazine
        /// Uses whent we need to transfer separately author id
        /// </summary>
        /// <param name="magazineDTO"></param>
        /// <param name="authorId"></param>
        /// <returns>Magazine instance</returns>
        public static Magazine ToMagazine(MagazineDTO magazineDTO, int authorId)
        {
            Magazine magazine = new Magazine()
            {
                MagazineId = magazineDTO.Id,
                MagazineName = magazineDTO.MagazineName,
                HostId = authorId,
                Cover = magazineDTO.Cover,
                Description = magazineDTO.Description,
                Tags = magazineDTO.Tags,
                Price = magazineDTO.Price
            };
            return magazine;
        }


        /// <summary>
        /// Method make covertation from MagazineDTO to Magazine
        /// </summary>
        /// <param name="magazineDTO"></param>
        /// <returns>Magazine instance</returns>
        public static Magazine ToMagazine(MagazineDTO magazineDTO)
        {
            Magazine magazine = new Magazine()
            {
                MagazineId = magazineDTO.Id,
                MagazineName = magazineDTO.MagazineName,
                HostId = magazineDTO.HostId,
                Cover = magazineDTO.Cover,
                Description = magazineDTO.Description,
                Tags = magazineDTO.Tags,
                Price = magazineDTO.Price
            };
            return magazine;
        }


        /// <summary>
        /// Method make covertation from Magazine to MagazineDTO
        /// </summary>
        /// <param name="magazineDTO"></param>
        /// <returns>MagazineDTO instance</returns>
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
