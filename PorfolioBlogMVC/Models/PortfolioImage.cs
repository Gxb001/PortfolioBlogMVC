using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PorfolioBlogMVC.Models;

public class PortfolioImage
{
    [Key] public int Id { get; set; }

    [Required] public required string Url { get; set; }

    [StringLength(200)] public string? Description { get; set; }

    public bool EstImagePrincipale { get; set; } = false;

    [ForeignKey(nameof(ElementPortfolio))] public int ElementPortfolioId { get; set; }

    // Propriété de navigation - EF la charge automatiquement
    public ElementPortfolio ElementPortfolio { get; set; } = null!;
}