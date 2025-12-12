using Microsoft.AspNetCore.Mvc;
using PorfolioBlogMVC.Data;

namespace PorfolioBlogMVC.Components;

public class CategoriesMenuViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public CategoriesMenuViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var categories = await Task.Run(() => _context.Categories.ToList());
        return View(categories);
    }
}