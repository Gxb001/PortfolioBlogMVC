using System;
using System.Security.Claims;
using System.Threading.Tasks;
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
        var articles = _context.Articles
            .Include(a => a.Auteur)
            .Include(a => a.Categorie);
        return View(await articles.ToListAsync());
    }

    // GET: Article/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var article = await _context.Articles
            .Include(a => a.Auteur)
            .Include(a => a.Categorie)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (article == null)
            return NotFound();

        return View(article);
    }

    // GET: Article/Create
    [Authorize] // Seuls les utilisateurs connectés peuvent créer un article
    public IActionResult Create()
    {
        ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Nom");
        return View();
    }

    // POST: Article/Create
    [HttpPost]
    [Authorize] // Seuls les utilisateurs connectés peuvent soumettre un article
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Titre,Contenu,ImagePrincipale,CategorieId")]
        Article article)
    {
        if (ModelState.IsValid)
        {
            // Assigner automatiquement l'ID de l'utilisateur connecté comme auteur
            article.AuteurId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            article.DatePublication = DateTime.UtcNow;

            _context.Add(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Recharger la liste des catégories en cas d'erreur
        ViewData["CategorieId"] = new SelectList(_context.Categories, "Id", "Nom", article.CategorieId);
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
        [Bind("Id,Titre,Contenu,ImagePrincipale,CategorieId")]
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

    private bool ArticleExists(int id)
    {
        return _context.Articles.Any(e => e.Id == id);
    }
}