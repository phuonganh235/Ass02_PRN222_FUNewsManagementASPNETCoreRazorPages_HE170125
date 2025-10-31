namespace DataAccess.Entities
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public int? ParentCategoryID { get; set; }
        public bool IsActive { get; set; }

        public ICollection<NewsArticle>? NewsArticles { get; set; }
    }
}
