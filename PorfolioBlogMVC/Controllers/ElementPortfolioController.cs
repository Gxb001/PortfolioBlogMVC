using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PorfolioBlogMVC.Data;
using PorfolioBlogMVC.Models;

namespace PorfolioBlogMVC.Controllers;

public class ElementPortfolioController : Controller
{
    private readonly ApplicationDbContext _context;

    public ElementPortfolioController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ElementPortfolio
    public async Task<IActionResult> Index()
    {
        var elements = await _context.ElementsPortfolio
            .Include(e => e.Createur)
            .Include(e => e.Images)
            .Include(e => e.Tags)
            .OrderByDescending(e => e.DateCreation)
            .ToListAsync();

        return View(elements);
    }

    // GET: ElementPortfolio/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();

        var element = await _context.ElementsPortfolio
            .Include(e => e.Createur)
            .Include(e => e.Images)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (element == null)
            return NotFound();

        return View(element);
    }

    // GET: ElementPortfolio/Create
    [Authorize]
    public IActionResult Create()
    {
        ViewBag.Tags = _context.Tags.ToList();
        return View();
    }

    // POST: ElementPortfolio/Create
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Titre,Description")] ElementPortfolio element,
        string imageUrls,
        int[] selectedTags)
    {
        // Retirer les erreurs de validation pour les propriétés de navigation
        ModelState.Remove("Createur");
        ModelState.Remove("Images");
        ModelState.Remove("Tags");

        if (ModelState.IsValid)
        {
            // Assigner l'utilisateur connecté
            element.CreateurId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            element.DateCreation = DateTime.UtcNow;

            _context.Add(element);
            await _context.SaveChangesAsync();

            // Ajouter les images
            if (!string.IsNullOrWhiteSpace(imageUrls))
            {
                var urls = imageUrls.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                for (var i = 0; i < urls.Length; i++)
                {
                    var url = urls[i].Trim();
                    if (!string.IsNullOrWhiteSpace(url))
                    {
                        var image = new PortfolioImage
                        {
                            Url = url,
                            ElementPortfolioId = element.Id,
                            EstImagePrincipale = i == 0 // Première image = principale
                        };
                        _context.PortfolioImages.Add(image);
                    }
                }
            }

            // Ajouter les tags
            if (selectedTags != null && selectedTags.Length > 0)
            {
                var tags = await _context.Tags.Where(t => selectedTags.Contains(t.Id)).ToListAsync();
                element.Tags = tags;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Tags = _context.Tags.ToList();
        return View(element);
    }

    // GET: ElementPortfolio/Edit/5
    [Authorize]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();

        var element = await _context.ElementsPortfolio
            .Include(e => e.Images)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (element == null)
            return NotFound();

        // Vérifier les permissions
        if (element.CreateurId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            return Forbid();

        ViewBag.Tags = _context.Tags.ToList();
        ViewBag.SelectedTags = element.Tags.Select(t => t.Id).ToArray();
        return View(element);
    }

    // POST: ElementPortfolio/Edit/5
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,Titre,Description,CreateurId,DateCreation")]
        ElementPortfolio element,
        string imageUrls,
        int[] selectedTags)
    {
        if (id != element.Id)
            return NotFound();

        // Vérifier les permissions
        var existingElement = await _context.ElementsPortfolio.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        if (existingElement == null)
            return NotFound();

        if (existingElement.CreateurId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            return Forbid();

        // Retirer les erreurs de validation pour les propriétés de navigation
        ModelState.Remove("Createur");
        ModelState.Remove("Images");
        ModelState.Remove("Tags");

        if (ModelState.IsValid)
        {
            try
            {
                // Conserver les valeurs originales
                element.CreateurId = existingElement.CreateurId;
                element.DateCreation = existingElement.DateCreation;

                _context.Update(element);

                // Gérer les images
                var oldImages = await _context.PortfolioImages.Where(i => i.ElementPortfolioId == id).ToListAsync();
                _context.PortfolioImages.RemoveRange(oldImages);

                if (!string.IsNullOrWhiteSpace(imageUrls))
                {
                    var urls = imageUrls.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                    for (var i = 0; i < urls.Length; i++)
                    {
                        var url = urls[i].Trim();
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            var image = new PortfolioImage
                            {
                                Url = url,
                                ElementPortfolioId = element.Id,
                                EstImagePrincipale = i == 0
                            };
                            _context.PortfolioImages.Add(image);
                        }
                    }
                }

                // Gérer les tags
                var elementToUpdate = await _context.ElementsPortfolio
                    .Include(e => e.Tags)
                    .FirstAsync(e => e.Id == id);

                elementToUpdate.Tags.Clear();
                if (selectedTags != null && selectedTags.Length > 0)
                {
                    var tags = await _context.Tags.Where(t => selectedTags.Contains(t.Id)).ToListAsync();
                    foreach (var tag in tags) elementToUpdate.Tags.Add(tag);
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElementPortfolioExists(element.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewBag.Tags = _context.Tags.ToList();
        ViewBag.SelectedTags = selectedTags;
        return View(element);
    }

    // GET: ElementPortfolio/Delete/5
    [Authorize]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var element = await _context.ElementsPortfolio
            .Include(e => e.Createur)
            .Include(e => e.Images)
            .Include(e => e.Tags)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (element == null)
            return NotFound();

        // Vérifier les permissions
        if (element.CreateurId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            return Forbid();

        return View(element);
    }

    // POST: ElementPortfolio/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var element = await _context.ElementsPortfolio
            .Include(e => e.Images)
            .FirstOrDefaultAsync(e => e.Id == id);

        if (element == null)
            return NotFound();

        // Vérifier les permissions
        if (element.CreateurId != User.FindFirstValue(ClaimTypes.NameIdentifier) && !User.IsInRole("Admin"))
            return Forbid();

        _context.ElementsPortfolio.Remove(element);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    private bool ElementPortfolioExists(int id)
    {
        return _context.ElementsPortfolio.Any(e => e.Id == id);
    }
}