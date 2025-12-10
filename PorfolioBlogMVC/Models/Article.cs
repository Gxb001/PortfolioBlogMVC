using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PorfolioBlogMVC.Models;

public class Article
{
    [Key] public int Id { get; set; }

    [Required] [StringLength(200)] public required string Titre { get; set; }

    [Required] public required string Contenu { get; set; }

    public string? ImagePrincipale { get; set; }

    public DateTime DatePublication { get; set; } = DateTime.UtcNow;

    // Auteur
    [ForeignKey(nameof(Auteur))] public required string AuteurId { get; set; }
    public required ApplicationUser Auteur { get; set; }

    // Catégorie
    public int? CategorieId { get; set; }
    public CategorieArticle? Categorie { get; set; }

    // Tags (Many-to-many)
    public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    // Commentaires
    public ICollection<Commentaire> Commentaires { get; set; } = new List<Commentaire>();
}