using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PorfolioBlogMVC.Models;

public class ElementPortfolio
{
    [Key] public int Id { get; set; }

    [Required] [StringLength(200)] public required string Titre { get; set; }

    [Required] public required string Description { get; set; }

    public DateTime DateCreation { get; set; } = DateTime.UtcNow;

    // Relation avec l'utilisateur
    [ForeignKey(nameof(Createur))] public required string CreateurId { get; set; }
    public required ApplicationUser Createur { get; set; }

    // Images (one-to-many)
    public ICollection<PortfolioImage> Images { get; set; } = new List<PortfolioImage>();
}