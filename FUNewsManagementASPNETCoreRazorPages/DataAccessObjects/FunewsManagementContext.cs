using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObjects;

public partial class FunewsManagementContext : DbContext
{
    public FunewsManagementContext() { }

    public FunewsManagementContext(DbContextOptions<FunewsManagementContext> options) : base(options) { }

    public virtual DbSet<SystemAccount> SystemAccounts { get; set; } = null!;
    public virtual DbSet<Category> Categories { get; set; } = null!;
    public virtual DbSet<Tag> Tags { get; set; } = null!;
    public virtual DbSet<NewsArticle> NewsArticles { get; set; } = null!;
    public virtual DbSet<NewsTag> NewsTags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // -------- SystemAccount ----------
        modelBuilder.Entity<SystemAccount>(entity =>
        {
            entity.ToTable("SystemAccount");
            entity.HasKey(e => e.AccountId);
            entity.Property(e => e.AccountId).HasColumnName("AccountID"); // smallint
            entity.Property(e => e.AccountName).HasMaxLength(100);
            entity.Property(e => e.AccountEmail).HasMaxLength(70);
            entity.Property(e => e.AccountPassword).HasMaxLength(70);
            entity.Property(e => e.AccountRole); // int

            entity.HasMany(e => e.CreatedNews)
                  .WithOne(n => n.CreatedBy)
                  .HasForeignKey(n => n.CreatedById)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.UpdatedNews)
                  .WithOne(n => n.UpdatedBy)
                  .HasForeignKey(n => n.UpdatedById)
                  .OnDelete(DeleteBehavior.ClientSetNull);
        });

        // -------- Category ----------
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");
            entity.HasKey(e => e.CategoryId);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID"); // smallint
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            // GIỮ NGUYÊN TÊN CỘT BỊ TYPO
            entity.Property(e => e.CategoryDesciption)
                  .HasColumnName("CategoryDesciption")
                  .HasMaxLength(250);

            entity.Property(e => e.ParentCategoryId).HasColumnName("ParentCategoryID");
            entity.Property(e => e.IsActive);

            entity.HasOne(e => e.ParentCategory)
                  .WithMany(e => e.SubCategories)
                  .HasForeignKey(e => e.ParentCategoryId)
                  .OnDelete(DeleteBehavior.ClientSetNull);
        });

        // -------- Tag ----------
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tag");
            entity.HasKey(e => e.TagId);
            entity.Property(e => e.TagId).HasColumnName("TagID");
            entity.Property(e => e.TagName).HasMaxLength(50);
            entity.Property(e => e.Note).HasMaxLength(400);
        });

        // -------- NewsArticle ----------
        modelBuilder.Entity<NewsArticle>(entity =>
        {
            entity.ToTable("NewsArticle");
            entity.HasKey(e => e.NewsArticleId);
            entity.Property(e => e.NewsArticleId)
                  .HasColumnName("NewsArticleID")
                  .HasMaxLength(20);

            entity.Property(e => e.NewsTitle).HasMaxLength(400);
            entity.Property(e => e.Headline).HasMaxLength(150).IsRequired();
            entity.Property(e => e.CreatedDate);
            entity.Property(e => e.NewsContent).HasMaxLength(4000);
            entity.Property(e => e.NewsSource).HasMaxLength(400);
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");       // smallint?
            entity.Property(e => e.NewsStatus);                                   // bit
            entity.Property(e => e.CreatedById).HasColumnName("CreatedByID");     // smallint
            entity.Property(e => e.UpdatedById).HasColumnName("UpdatedByID");     // smallint
            entity.Property(e => e.ModifiedDate);

            entity.HasOne(e => e.Category)
                  .WithMany(c => c.NewsArticles)
                  .HasForeignKey(e => e.CategoryId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.CreatedBy)
                  .WithMany(a => a.CreatedNews)
                  .HasForeignKey(e => e.CreatedById)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.UpdatedBy)
                  .WithMany(a => a.UpdatedNews)
                  .HasForeignKey(e => e.UpdatedById)
                  .OnDelete(DeleteBehavior.ClientSetNull);
        });

        // -------- NewsTag ----------
        modelBuilder.Entity<NewsTag>(entity =>
        {
            entity.ToTable("NewsTag");
            entity.HasKey(e => new { e.NewsArticleId, e.TagId });

            entity.Property(e => e.NewsArticleId)
                  .HasColumnName("NewsArticleID")
                  .HasMaxLength(20);

            entity.Property(e => e.TagId).HasColumnName("TagID");

            entity.HasOne(e => e.NewsArticle)
                  .WithMany(n => n.NewsTags)
                  .HasForeignKey(e => e.NewsArticleId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Tag)
                  .WithMany(t => t.NewsTags)
                  .HasForeignKey(e => e.TagId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
