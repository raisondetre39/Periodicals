using Periodicals.DAL.Publishings;
using System;
using System.Collections.Generic;

namespace Periodical.BL.DataTemporaryModels
{
    public class MagazineDTO
    {
        public int Id { get; set; }
        public string MagazineName { get; set; }
        public DateTime PublishDate { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public string Cover { get; set; }
        public List<Tag> Tags { get; set; }
        public int HostId { get; set; }
    }
}
