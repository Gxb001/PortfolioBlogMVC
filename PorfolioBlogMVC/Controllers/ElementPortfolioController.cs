using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        var applicationDbContext = _context.ElementsPortfolio.Include(e => e.Createur);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: ElementPortfolio/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var elementPortfolio = await _context.ElementsPortfolio
            .Include(e => e.Createur)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (elementPortfolio == null) return NotFound();

        return View(elementPortfolio);
    }

    // GET: ElementPortfolio/Create
    public IActionResult Create()
    {
        ViewData["CreateurId"] = new SelectList(_context.Users, "Id", "Id");
        return View();
    }

    // POST: ElementPortfolio/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,Titre,Description,DateCreation,CreateurId")]
        ElementPortfolio elementPortfolio)
    {
        if (ModelState.IsValid)
        {
            _context.Add(elementPortfolio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CreateurId"] = new SelectList(_context.Users, "Id", "Id", elementPortfolio.CreateurId);
        return View(elementPortfolio);
    }

    // GET: ElementPortfolio/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var elementPortfolio = await _context.ElementsPortfolio.FindAsync(id);
        if (elementPortfolio == null) return NotFound();
        ViewData["CreateurId"] = new SelectList(_context.Users, "Id", "Id", elementPortfolio.CreateurId);
        return View(elementPortfolio);
    }

    // POST: ElementPortfolio/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id,
        [Bind("Id,Titre,Description,DateCreation,CreateurId")]
        ElementPortfolio elementPortfolio)
    {
        if (id != elementPortfolio.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(elementPortfolio);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ElementPortfolioExists(elementPortfolio.Id))
                    return NotFound();
                else
                    throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CreateurId"] = new SelectList(_context.Users, "Id", "Id", elementPortfolio.CreateurId);
        return View(elementPortfolio);
    }

    // GET: ElementPortfolio/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var elementPortfolio = await _context.ElementsPortfolio
            .Include(e => e.Createur)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (elementPortfolio == null) return NotFound();

        return View(elementPortfolio);
    }

    // POST: ElementPortfolio/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var elementPortfolio = await _context.ElementsPortfolio.FindAsync(id);
        if (elementPortfolio != null) _context.ElementsPortfolio.Remove(elementPortfolio);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ElementPortfolioExists(int id)
    {
        return _context.ElementsPortfolio.Any(e => e.Id == id);
    }
}