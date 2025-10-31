﻿namespace DataAccess.Entities
{
    public class Tag
    {
        public int TagID { get; set; }
        public string? TagName { get; set; }

        public ICollection<NewsArticleTag>? NewsArticleTags { get; set; }
    }
}
