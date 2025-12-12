using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PorfolioBlogMVC.Data;
using PorfolioBlogMVC.Models;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// DB
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Identity
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
        
        // Créer le rôle "Admin" et "Standard" s'ils n'existent pas
        var roles = new[] { "Admin", "Standard" };
        foreach (var role in roles)
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));

        // Utilisateur Admin
        var adminEmail = "admintest@admin.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        var logger = services.GetRequiredService<ILogger<Program>>();

        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "GabAdmin",
                Email = adminEmail,
                Nom = "Ferrer",
                Prenom = "Gabriel",
                EmailConfirmed = true,
                LockoutEnabled = false
            };

            var createResult = await userManager.CreateAsync(adminUser, "Azerty31?");
            if (!createResult.Succeeded)
            {
                var errorMsgs = "";
                foreach (var err in createResult.Errors) errorMsgs += err.Description + " ; ";
                logger.LogWarning("Échec création Admin : {Errors}", errorMsgs);

                var existing = await userManager.FindByEmailAsync(adminEmail);
                if (existing != null)
                {
                    adminUser = existing;
                    if (!await userManager.HasPasswordAsync(adminUser))
                    {
                        var pwdResult = await userManager.AddPasswordAsync(adminUser, "Azerty31?");
                        if (!pwdResult.Succeeded)
                        {
                            var pwdErr = "";
                            foreach (var pe in pwdResult.Errors) pwdErr += pe.Description + " ; ";
                            logger.LogWarning("Échec ajout mot de passe Admin : {Errors}", pwdErr);
                        }
                    }
                }
            }
        }
        else
        {
            adminUser.EmailConfirmed = true;
            adminUser.LockoutEnabled = false;
            await userManager.UpdateAsync(adminUser);
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
            if (!roleResult.Succeeded)
            {
                var roleErr = "";
                foreach (var re in roleResult.Errors) roleErr += re.Description + " ; ";
                logger.LogWarning("Échec ajout rôle Admin : {Errors}", roleErr);
            }
        }

        // Utilisateur Standard
        var standardEmail = "user@example.com";
        var standardUser = await userManager.FindByEmailAsync(standardEmail);
        if (standardUser == null)
        {
            standardUser = new ApplicationUser
            {
                UserName = "FoucaultStandard",
                Email = standardEmail,
                Nom = "Lapeze",
                Prenom = "Foucault",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(standardUser, "Azerty31!");
        }

        // Ajouter le rôle Standard si nécessaire
        if (!await userManager.IsInRoleAsync(standardUser, "Standard"))
            await userManager.AddToRoleAsync(standardUser, "Standard");


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