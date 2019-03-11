using Periodicals.DAL.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Periodicals.DAL.Publishings
{
    public class Magazine
    {
        public int MagazineId { get; set; }

        public string MagazineName { get; set; }

        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [DataType(DataType.Currency)]
        public int Price { get; set; }

        public string Cover { get; set; }

        public virtual List<Tag> Tags { get; set; }

        public virtual List<Host> Hosts { get; set; }

        public int? HostId { get; set; }

        public Host Host { get; set; }

        public Magazine()
        {
            Tags = new List<Tag>();
            Hosts = new List<Host>(); 
        }
    }
}
