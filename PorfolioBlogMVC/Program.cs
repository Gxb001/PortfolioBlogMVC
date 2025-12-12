using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PorfolioBlogMVC.Data;
using PorfolioBlogMVC.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection") ??
                         throw new InvalidOperationException("Connection string 'PorfolioBlogMVCContext' not found.")));

// DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 8;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Créer les rôles et les utilisateurs par défaut au démarrage de l'application
// simplifie les tests
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var context = services.GetRequiredService<ApplicationDbContext>();

        // Créer les rôles "Admin" et "Visiteur" s'ils n'existent pas
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));

        if (!await roleManager.RoleExistsAsync("Visiteur"))
            await roleManager.CreateAsync(new IdentityRole("Visiteur"));

        // Créer un seul utilisateur admin
        var adminUser = new ApplicationUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            Nom = "Ferrer",
            Prenom = "Gabriel",
            EmailConfirmed = false
        };
        var adminPassword = "Azerty31!";
        var adminUserExists = await userManager.FindByEmailAsync(adminUser.Email);
        if (adminUserExists == null)
        {
            var createAdmin = await userManager.CreateAsync(adminUser, adminPassword);
            if (createAdmin.Succeeded) await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        // Créer un seul utilisateur visiteur
        var visiteurUser = new ApplicationUser
        {
            UserName = "visiteur@example.com",
            Email = "visiteur@example.com",
            Nom = "Dupont",
            Prenom = "Jean",
            EmailConfirmed = false
        };
        var visiteurPassword = "Azerty31!";
        var visiteurUserExists = await userManager.FindByEmailAsync(visiteurUser.Email);
        if (visiteurUserExists == null)
        {
            var createVisiteur = await userManager.CreateAsync(visiteurUser, visiteurPassword);
            if (createVisiteur.Succeeded) await userManager.AddToRoleAsync(visiteurUser, "Visiteur");
        }

        await context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erreur lors de la création des rôles/utilisateurs.");
    }
}


// Créer les Categories et Tags 
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    if (!context.Tags.Any())
    {
        context.Tags.AddRange(
            new Tag { Nom = "C#", Couleur = "#178600" },
            new Tag { Nom = "ASP.NET", Couleur = "#512BD4" },
            new Tag { Nom = "EntityFramework", Couleur = "#3178C6" },
            new Tag { Nom = "JavaScript", Couleur = "#F7DF1E" },
            new Tag { Nom = "Design", Couleur = "#FF6F61" }
        );
        context.SaveChanges();
    }

    if (!context.Categories.Any())
    {
        context.Categories.AddRange(
            new CategorieArticle { Nom = "Programmation" },
            new CategorieArticle { Nom = "Web" },
            new CategorieArticle { Nom = "Base de données" },
            new CategorieArticle { Nom = "Design" },
            new CategorieArticle { Nom = "DevOps" }
        );
        context.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();