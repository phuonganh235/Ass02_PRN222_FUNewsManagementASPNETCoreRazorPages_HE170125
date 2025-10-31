﻿namespace DataAccess.Entities
{
    public class Tag
    {
        public int TagId { get; set; }
        public string TagName { get; set; } = string.Empty;

        public ICollection<NewsTag>? NewsTags { get; set; }
    }
}
