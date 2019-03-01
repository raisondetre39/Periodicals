using System.Collections.Generic;

namespace Periodicals.DAL.Publishings
{
    public class Tag
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public List<Magazine> Magazines { get; set; }

        public Tag()
        {
            Magazines = new List<Magazine>();
        }
    }
}
