using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PorfolioBlogMVC.Models;

namespace PorfolioBlogMVC.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Tables principales
    public DbSet<Article> Articles { get; set; }
    public DbSet<ElementPortfolio> ElementsPortfolio { get; set; }
    public DbSet<PortfolioImage> PortfolioImages { get; set; }
    public DbSet<CategorieArticle> Categories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<Commentaire> Commentaires { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Many-to-Many Article <-> Tag
        modelBuilder.Entity<Article>()
            .HasMany(a => a.Tags)
            .WithMany(t => t.Articles)
            .UsingEntity<Dictionary<string, object>>(
                "ArticleTag",
                j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Article>().WithMany().HasForeignKey("ArticleId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasKey("ArticleId", "TagId")
            );

        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.Nom)
            .HasMaxLength(150);
        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.Prenom)
            .HasMaxLength(150);

        modelBuilder.Entity<Tag>()
            .Property(t => t.Nom)
            .IsRequired()
            .HasMaxLength(50);

        modelBuilder.Entity<CategorieArticle>()
            .Property(c => c.Nom)
            .IsRequired()
            .HasMaxLength(100);

        modelBuilder.Entity<Article>()
            .Property(a => a.Titre)
            .IsRequired()
            .HasMaxLength(200);

        modelBuilder.Entity<ElementPortfolio>()
            .Property(e => e.Titre)
            .IsRequired()
            .HasMaxLength(200);
    }
}