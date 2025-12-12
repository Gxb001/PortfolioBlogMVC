using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PorfolioBlogMVC.Data;
using PorfolioBlogMVC.Models;

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

        // Créer le rôle "Admin" s'il n'existe pas
        // Créer le rôle "Admin" et "Standard" s'ils n'existent pas
        var roles = new[] { "Admin", "Standard" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        // Utilisateur Admin
        var adminEmail = "admin@example.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "GabAdmin",
                Email = adminEmail,
                Nom = "Ferrer",
                Prenom = "Gabriel",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, "Azerty31!");
        }
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            await userManager.AddToRoleAsync(adminUser, "Admin");
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
        {
            await userManager.AddToRoleAsync(standardUser, "Standard");
        }


        await context.SaveChangesAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Erreur lors de la création des rôles/utilisateurs.");
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();