using System.ComponentModel.DataAnnotations;

namespace PorfolioBlogMVC.Models;

public class Tag
{
    [Key] public int Id { get; set; }

    [Required] [StringLength(50)] public required string Nom { get; set; }

    [StringLength(7)] public string? Couleur { get; set; } // Format: #RRGGBB

    // Relations
    public ICollection<Article> Articles { get; set; } = new List<Article>();
    public ICollection<ElementPortfolio> ElementsPortfolio { get; set; } = new List<ElementPortfolio>();
}