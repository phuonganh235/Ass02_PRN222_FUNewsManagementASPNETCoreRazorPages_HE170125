using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class NewsTag
    {
        public string NewsArticleId { get; set; } = string.Empty;
        public int TagId { get; set; }
        public NewsArticle NewsArticle { get; set; } = null!;
        public Tag Tag { get; set; } = null!;
    }
}
