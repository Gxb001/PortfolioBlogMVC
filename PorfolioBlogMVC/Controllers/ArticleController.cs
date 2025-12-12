using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PorfolioBlogMVC.Data;
using PorfolioBlogMVC.Models;

namespace PorfolioBlogMVC.Controllers;

public class ArticleController : Controller
{
    private readonly ApplicationDbContext _context;

    public ArticleController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Article
    public async Task<IActionResult> Index()
    {
        var articles = await _context.Articles
            .Include(a => a.Auteur)
            .Include(a => a.Categorie)
            .Include(a => a.Commentaires)
            .OrderByDescending(a => a.DatePublication)
            .ToListAsync();
        return View(articles);
    }

    // GET: Article/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var article = await _context.Articles
            .Include(a => a.Auteur)
            .Include(a => a.Categorie)
            .Include(a => a.Commentaires)
            .ThenInclude(c => c.Auteur)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (article == null)
            return NotFound();

        return View(article);
    }

    // GET: Article/Create (For AJAX)
    [Authorize]
    public IActionResult Create()
    {
        ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Nom");
        // if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") return PartialView("_CreatePartial");
        return View();
    }

    // POST: Article/Create
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Titre,Contenu,ImagePrincipale,CategorieId")]
        Article article)
    {
        if (ModelState.IsValid)
        {
            article.AuteurId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            article.DatePublication = DateTime.UtcNow;

            _context.Add(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Nom", article.CategorieId);
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest") return PartialView("_CreatePartial", article);
        return View(article);
    }

    // GET: Article/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var article = await _context.Articles.FindAsync(id);
        if (article == null)
            return NotFound();

        // Vérifier que l'utilisateur est l'auteur ou un admin
        if (article.AuteurId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            return Forbid();

        ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Nom", article.CategorieId);
        return View(article);
    }

    // POST: Article/Edit/5
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Titre,Contenu,ImagePrincipale,CategorieId,AuteurId,DatePublication")]
        Article article)
    {
        if (id != article.Id)
            return NotFound();

        // Vérifier que l'utilisateur est l'auteur ou un admin
        var existingArticle = await _context.Articles.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        if (existingArticle == null)
            return NotFound();

        if (existingArticle.AuteurId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            return Forbid();

        if (ModelState.IsValid)
        {
            try
            {
                // Conserver les valeurs originales
                article.AuteurId = existingArticle.AuteurId;
                article.DatePublication = existingArticle.DatePublication;

                _context.Update(article);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(article.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Nom", article.CategorieId);
        return View(article);
    }

    // GET: Article/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var article = await _context.Articles
            .Include(a => a.Auteur)
            .Include(a => a.Categorie)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (article == null)
            return NotFound();

        // Vérifier que l'utilisateur est l'auteur ou un admin
        if (article.AuteurId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            return Forbid();

        return View(article);
    }

    // POST: Article/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var article = await _context.Articles.FindAsync(id);
        if (article == null)
            return NotFound();

        // Vérifier que l'utilisateur est l'auteur ou un admin
        if (article.AuteurId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            return Forbid();

        _context.Articles.Remove(article);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    // POST: Article/AddComment
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddComment(int articleId, string contenu)
    {
        if (string.IsNullOrWhiteSpace(contenu))
            return BadRequest("Le contenu du commentaire est requis.");

        var article = await _context.Articles.FindAsync(articleId);
        if (article == null)
            return NotFound();

        var auteurId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var auteur = await _context.Users.FindAsync(auteurId);

        if (auteur == null)
            return Unauthorized();

        var commentaire = new Commentaire
        {
            Contenu = contenu,
            ArticleId = articleId,
            AuteurId = auteurId,
            DatePoste = DateTime.UtcNow
        };

        _context.Commentaires.Add(commentaire);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = articleId });
    }

    // POST: Article/DeleteComment
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteComment(int commentId, int articleId)
    {
        var commentaire = await _context.Commentaires.FindAsync(commentId);
        if (commentaire == null)
            return NotFound();

        // Vérifier que l'utilisateur est l'auteur du commentaire ou un admin
        if (commentaire.AuteurId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            return Forbid();

        _context.Commentaires.Remove(commentaire);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Details), new { id = articleId });
    }

    private bool ArticleExists(int id)
    {
        return _context.Articles.Any(e => e.Id == id);
    }
}