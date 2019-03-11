using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Periodicals.DAL.Publishings
{
    public class TagMagazine
    {
        public int TagMagazineId { get; set; }

        public int? TagId { get; set; }

        public int? MagazineId { get; set; }

        public virtual Tag Tag { get; set; }

        public virtual Magazine Magazine { get; set; }
    }
}
