using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PorfolioBlogMVC.Models;

public class RegisterModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public RegisterModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
            var result = await _userManager.CreateAsync(user, Input.Password);

            if (result.Succeeded)
            {
                // Crée le rôle "Standard" s'il n'existe pas
                if (!await _roleManager.RoleExistsAsync("Standard"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Standard"));
                }

                // Ajouter l'utilisateur au rôle Standard si ce n'est pas déjà fait
                if (!await _userManager.IsInRoleAsync(user, "Standard"))
                {
                    await _userManager.AddToRoleAsync(user, "Standard");
                }

                // Connexion automatique après inscription
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect("~/");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return Page();
    }
}
