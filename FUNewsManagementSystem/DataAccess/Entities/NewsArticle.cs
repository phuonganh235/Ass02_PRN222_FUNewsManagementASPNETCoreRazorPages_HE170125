using System;
using System.Collections.Generic;
namespace DataAccess.Entities
{
    public class NewsArticle
    {
        public string NewsArticleID { get; set; } = default!; // nvarchar(20)
        public string NewsTitle { get; set; } = default!;     // nvarchar(400)
        public string? Headline { get; set; }                 // nvarchar(150)
        public DateTime CreatedDate { get; set; }
        public string? NewsContent { get; set; }              // nvarchar(4000)
        public string? NewsSource { get; set; }               // nvarchar(400)
        public short CategoryID { get; set; }
        public bool NewsStatus { get; set; }
        public short CreatedByID { get; set; }
        public short? UpdatedByID { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Category? Category { get; set; }
        public SystemAccount? CreatedBy { get; set; }
        public SystemAccount? UpdatedBy { get; set; }
        public ICollection<NewsTag>? NewsTags { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}