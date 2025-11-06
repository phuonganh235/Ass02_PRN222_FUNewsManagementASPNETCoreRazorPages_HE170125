namespace DataAccess.Entities
{
    public class Category
    {
        public short CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? CategoryDescription { get; set; }
        public short? ParentCategoryID { get; set; }
        public bool IsActive { get; set; }

        public ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
    }
}
