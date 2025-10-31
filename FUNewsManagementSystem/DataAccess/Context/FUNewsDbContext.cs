using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class FUNewsDbContext : DbContext
    {
        public FUNewsDbContext(DbContextOptions<FUNewsDbContext> options) : base(options)
        {
        }

        public DbSet<SystemAccount> SystemAccounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<NewsArticleTag> NewsArticleTags { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            base.OnModelCreating(mb);

            // PK
            mb.Entity<SystemAccount>().HasKey(a => a.AccountID);
            mb.Entity<Category>().HasKey(c => c.CategoryID);
            mb.Entity<Tag>().HasKey(t => t.TagID);
            mb.Entity<NewsArticle>().HasKey(n => n.NewsArticleID);
            mb.Entity<Comment>().HasKey(c => c.CommentID);
            mb.Entity<NewsArticleTag>().HasKey(nt => new { nt.NewsArticleID, nt.TagID });

            // Relationship NewsArticle ↔ Category
            mb.Entity<NewsArticle>()
                .HasOne(n => n.Category)
                .WithMany(c => c.NewsArticles)
                .HasForeignKey(n => n.CategoryID);

            // Relationship NewsArticle ↔ SystemAccount (CreatedBy)
            mb.Entity<NewsArticle>()
                .HasOne(n => n.CreatedBy)
                .WithMany(a => a.CreatedArticles)
                .HasForeignKey(n => n.CreatedByID);

            // Relationship Comment ↔ NewsArticle
            mb.Entity<Comment>()
                .HasOne(c => c.NewsArticle)
                .WithMany(n => n.Comments)
                .HasForeignKey(c => c.NewsArticleID);

            // Many-to-many NewsArticleTag
            mb.Entity<NewsArticleTag>()
                .HasOne(nt => nt.NewsArticle)
                .WithMany(n => n.NewsArticleTags)
                .HasForeignKey(nt => nt.NewsArticleID);

            mb.Entity<NewsArticleTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NewsArticleTags)
                .HasForeignKey(nt => nt.TagID);
        }
    }
}
