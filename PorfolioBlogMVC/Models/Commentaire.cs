using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PorfolioBlogMVC.Models;

public class Commentaire
{
    [Key] public int Id { get; set; }

    [Required] public required string Contenu { get; set; }

    public DateTime DatePoste { get; set; } = DateTime.UtcNow;

    // Auteur du commentaire
    [ForeignKey(nameof(Auteur))] public required string AuteurId { get; set; }
    public required ApplicationUser Auteur { get; set; }

    // Article lié
    [ForeignKey(nameof(Article))] public int ArticleId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)] public required Article Article { get; set; }
}