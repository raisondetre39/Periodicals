using System.Collections.Generic;

namespace Periodicals.DAL.Publishings
{
    /// <summary>
    ///  Class creates properties to work with tags table
    /// </summary>
    public class Tag
    {
        public int TagId { get; set; }

        public string TagName { get; set; }

        public virtual List<Magazine> Magazines { get; set; }

        public Tag()
        {
            Magazines = new List<Magazine>();
        }
    }
}
