using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PorfolioBlogMVC.Models;

public class PortfolioImage
{
    [Key] public int Id { get; set; }

    [Required] public required string Url { get; set; }

    [ForeignKey(nameof(ElementPortfolio))] public int ElementPortfolioId { get; set; }

    public required ElementPortfolio ElementPortfolio { get; set; }
}