using System;
using System.Collections.Generic;

namespace BusinessObjects;

public partial class SystemAccount
{
    public short AccountId { get; set; }

    public string? AccountName { get; set; }

    public string? AccountEmail { get; set; } = string.Empty;

    public int? AccountRole { get; set; }

    public string? AccountPassword { get; set; } = string.Empty ;

    public ICollection<NewsArticle> CreatedNews { get; set; } = new List<NewsArticle>();
    public ICollection<NewsArticle> UpdatedNews { get; set; } = new List<NewsArticle>();
}
