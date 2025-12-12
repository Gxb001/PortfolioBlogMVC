using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PorfolioBlogMVC.Data;
using PorfolioBlogMVC.Models;

namespace PorfolioBlogMVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        // Récupérer les 6 derniers articles
        var latestArticles = await _context.Articles
            .Include(a => a.Auteur)
            .Include(a => a.Categorie)
            .Include(a => a.Commentaires)
            .OrderByDescending(a => a.DatePublication)
            .Take(6)
            .ToListAsync();

        // Récupérer les 6 derniers projets de portfolio
        var latestPortfolios = await _context.ElementsPortfolio
            .Include(p => p.Createur)
            .Include(p => p.Images)
            .Include(p => p.Tags)
            .OrderByDescending(p => p.DateCreation)
            .Take(6)
            .ToListAsync();

        ViewBag.LatestArticles = latestArticles;
        ViewBag.LatestPortfolios = latestPortfolios;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}