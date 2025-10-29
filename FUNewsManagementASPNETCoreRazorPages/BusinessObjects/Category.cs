using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class Category
{
    public short CategoryId { get; set; }

    public string CategoryName { get; set; } = string.Empty;

    public string CategoryDesciption { get; set; } = string.Empty;

    public short? ParentCategoryId { get; set; }

    public bool? IsActive { get; set; }

    public Category? ParentCategory { get; set; }
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public ICollection<NewsArticle> NewsArticles { get; set; } = new List<NewsArticle>();
}
