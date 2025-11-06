using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class FUNewsDbContext : DbContext
    {
        public FUNewsDbContext(DbContextOptions<FUNewsDbContext> options) : base(options) { }

        public DbSet<SystemAccount> SystemAccounts => Set<SystemAccount>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<NewsArticle> NewsArticles => Set<NewsArticle>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<NewsTag> NewsTags => Set<NewsTag>();
        public DbSet<Comment> Comments => Set<Comment>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemAccount>().HasKey(a => a.AccountID);

            modelBuilder.Entity<Category>().HasKey(c => c.CategoryID);

            modelBuilder.Entity<NewsArticle>().HasKey(n => n.NewsArticleID);

            modelBuilder.Entity<NewsArticle>()
                .HasOne(n => n.Category)
                .WithMany(c => c.NewsArticles)
                .HasForeignKey(n => n.CategoryID);

            modelBuilder.Entity<NewsArticle>()
                .HasOne(n => n.CreatedBy)
                .WithMany(a => a.NewsArticles)
                .HasForeignKey(n => n.CreatedByID);

            modelBuilder.Entity<NewsTag>()
                .HasKey(nt => new { nt.NewsArticleID, nt.TagID });

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.NewsArticle)
                .WithMany(n => n.NewsTags)
                .HasForeignKey(nt => nt.NewsArticleID);

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NewsTags)
                .HasForeignKey(nt => nt.TagID);

            modelBuilder.Entity<Comment>().HasKey(c => c.CommentID);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.NewsArticle)
                .WithMany(n => n.Comments)
                .HasForeignKey(c => c.NewsArticleID);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Account)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => c.AccountID);
        }
    }
}
