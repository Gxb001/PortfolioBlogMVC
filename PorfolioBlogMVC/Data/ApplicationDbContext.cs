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

        // Configuration Article
        modelBuilder.Entity<Article>(entity =>
        {
            entity.Property(a => a.Titre)
                .IsRequired()
                .HasMaxLength(200);

            // Relation avec Auteur
            entity.HasOne(a => a.Auteur)
                .WithMany()
                .HasForeignKey(a => a.AuteurId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relation avec Categorie
            entity.HasOne(a => a.Categorie)
                .WithMany(c => c.Articles)
                .HasForeignKey(a => a.CategorieId)
                .OnDelete(DeleteBehavior.SetNull);

            // Many-to-Many Article <-> Tag
            entity.HasMany(a => a.Tags)
                .WithMany(t => t.Articles)
                .UsingEntity<Dictionary<string, object>>(
                    "ArticleTag",
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<Article>().WithMany().HasForeignKey("ArticleId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasKey("ArticleId", "TagId")
                );

            // Cascade delete pour les commentaires
            entity.HasMany(a => a.Commentaires)
                .WithOne(c => c.Article)
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration Commentaire
        modelBuilder.Entity<Commentaire>(entity =>
        {
            entity.HasOne(c => c.Auteur)
                .WithMany()
                .HasForeignKey(c => c.AuteurId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(c => c.Article)
                .WithMany(a => a.Commentaires)
                .HasForeignKey(c => c.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration ElementPortfolio
        modelBuilder.Entity<ElementPortfolio>(entity =>
        {
            entity.Property(e => e.Titre)
                .IsRequired()
                .HasMaxLength(200);

            // Relation avec Createur
            entity.HasOne(e => e.Createur)
                .WithMany()
                .HasForeignKey(e => e.CreateurId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cascade delete pour les images
            entity.HasMany(e => e.Images)
                .WithOne(i => i.ElementPortfolio)
                .HasForeignKey(i => i.ElementPortfolioId)
                .OnDelete(DeleteBehavior.Cascade);

            // Many-to-Many ElementPortfolio <-> Tag
            entity.HasMany(e => e.Tags)
                .WithMany(t => t.ElementsPortfolio)
                .UsingEntity<Dictionary<string, object>>(
                    "PortfolioTag",
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId").OnDelete(DeleteBehavior.Cascade),
                    j => j.HasOne<ElementPortfolio>().WithMany().HasForeignKey("ElementPortfolioId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j.HasKey("ElementPortfolioId", "TagId")
                );
        });

        // Configuration PortfolioImage
        modelBuilder.Entity<PortfolioImage>(entity =>
        {
            entity.HasOne(i => i.ElementPortfolio)
                .WithMany(e => e.Images)
                .HasForeignKey(i => i.ElementPortfolioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration Tag
        modelBuilder.Entity<Tag>(entity =>
        {
            entity.Property(t => t.Nom)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(t => t.Nom).IsUnique();
        });

        // Configuration CategorieArticle
        modelBuilder.Entity<CategorieArticle>(entity =>
        {
            entity.Property(c => c.Nom)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasIndex(c => c.Nom).IsUnique();

            entity.HasMany(c => c.Articles)
                .WithOne(a => a.Categorie)
                .HasForeignKey(a => a.CategorieId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Configuration ApplicationUser
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.Nom).HasMaxLength(150);
            entity.Property(u => u.Prenom).HasMaxLength(150);
        });
    }
}