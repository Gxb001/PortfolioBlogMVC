using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PorfolioBlogMVC.Data;
using PorfolioBlogMVC.Models;

namespace PorfolioBlogMVC.Controllers;

[Authorize(Roles = "Admin")]
public class CategorieArticleController : Controller
{
    private readonly ApplicationDbContext _context;

    public CategorieArticleController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: CategorieArticle
    public async Task<IActionResult> Index()
    {
        return View(await _context.Categories.ToListAsync());
    }

    // GET: CategorieArticle/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var categorieArticle = await _context.Categories
            .FirstOrDefaultAsync(m => m.Id == id);
        if (categorieArticle == null) return NotFound();

        return View(categorieArticle);
    }

    // GET: CategorieArticle/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CategorieArticle/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nom")] CategorieArticle categorieArticle)
    {
        if (ModelState.IsValid)
        {
            _context.Add(categorieArticle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(categorieArticle);
    }

    // GET: CategorieArticle/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var categorieArticle = await _context.Categories.FindAsync(id);
        if (categorieArticle == null) return NotFound();
        return View(categorieArticle);
    }

    // POST: CategorieArticle/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nom")] CategorieArticle categorieArticle)
    {
        if (id != categorieArticle.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(categorieArticle);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategorieArticleExists(categorieArticle.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(categorieArticle);
    }

    // GET: CategorieArticle/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var categorieArticle = await _context.Categories
            .FirstOrDefaultAsync(m => m.Id == id);
        if (categorieArticle == null) return NotFound();

        return View(categorieArticle);
    }

    // POST: CategorieArticle/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var categorieArticle = await _context.Categories.FindAsync(id);
        if (categorieArticle != null) _context.Categories.Remove(categorieArticle);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategorieArticleExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }
}