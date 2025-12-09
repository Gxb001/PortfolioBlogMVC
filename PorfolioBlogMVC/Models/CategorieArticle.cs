using System.ComponentModel.DataAnnotations;

namespace PorfolioBlogMVC.Models;

public class CategorieArticle
{
    [Key] public int Id { get; set; }

    [Required] [StringLength(100)] public required string Nom { get; set; }

    public ICollection<Article> Articles { get; set; } = new List<Article>();
}