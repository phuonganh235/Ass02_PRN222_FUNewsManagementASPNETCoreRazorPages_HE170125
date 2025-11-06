namespace DataAccess.Entities
{
    public class Tag
    {
        public int TagID { get; set; }
        public string? TagName { get; set; }
        public string? Note { get; set; }

        public ICollection<NewsTag> NewsTags { get; set; } = new List<NewsTag>();
    }
}
