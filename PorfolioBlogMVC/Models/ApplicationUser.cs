using Microsoft.AspNetCore.Identity;

namespace PorfolioBlogMVC.Models;

public class ApplicationUser : IdentityUser
{
    // ici on peut ajouter des prorpiétés supplémentaires pour l'utilisateur
    // par exemple :
    public string? Nom { get; set; }
    public string? Prenom { get; set; }

    // Propriétés de navigation pour les relations
    public ICollection<Article> Articles { get; set; } = new List<Article>();
    public ICollection<ElementPortfolio> ElementsPortfolio { get; set; } = new List<ElementPortfolio>();
}