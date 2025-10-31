using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Context
{
    public class FUNewsDbContext : DbContext
    {
        public FUNewsDbContext(DbContextOptions<FUNewsDbContext> options) : base(options) { }

        public DbSet<SystemAccount> SystemAccounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---- Many-to-Many: NewsArticle <-> Tag ----
            modelBuilder.Entity<NewsTag>()
                .HasKey(nt => new { nt.NewsId, nt.TagId });

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.NewsArticle)
                .WithMany(n => n.NewsTags)
                .HasForeignKey(nt => nt.NewsId);

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NewsTags)
                .HasForeignKey(nt => nt.TagId);

            // ---- NewsArticle <-> SystemAccount ----
            modelBuilder.Entity<NewsArticle>()
                .HasOne(n => n.CreatedBy)
                .WithMany(a => a.CreatedArticles)
                .HasForeignKey(n => n.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
