namespace Periodicals.DAL.Publishings
{
    /// <summary>
    ///  Class creates coonection between magazines list in tag class and tags list in magazines class
    /// </summary>
    public class TagMagazine
    {
        public int TagMagazineId { get; set; }

        public int? TagId { get; set; }

        public int? MagazineId { get; set; }

        public virtual Tag Tag { get; set; }

        public virtual Magazine Magazine { get; set; }
    }
}
