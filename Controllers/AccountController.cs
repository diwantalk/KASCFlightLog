using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KASCFlightLog.Models;
using KASCFlightLog.Models.ViewModels;

namespace KASCFlightLog.Controllers
{
    public class AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager) : Controller
    {
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null && !user.IsValidated)
                {
                    ModelState.AddModelError(string.Empty, "Your account is pending validation.");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(model.Email,
                    model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    if (user != null)
                    {
                        if (await userManager.IsInRoleAsync(user, "Admin"))
                            return RedirectToAction("Index", "Admin");
                        else if (await userManager.IsInRoleAsync(user, "Staff"))
                            return RedirectToAction("Index", "Staff");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FullName.Split(' ')[0],
                    LastName = model.FullName.Split(' ').Length > 1
                        ? string.Join(" ", model.FullName.Split(' ').Skip(1))
                        : string.Empty,
                    IsValidated = false,
                    Notes = model.Notes,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "RegularUser");

                    // Optionally sign in the user immediately
                    // await signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction(nameof(RegisterSuccess));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RegisterSuccess()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null || !user.IsValidated)
                {
                    // Don't reveal that the user does not exist or is not validated
                    return RedirectToAction(nameof(ForgotPasswordConfirmation));
                }

                // For now, just redirect to confirmation
                // In a real application, you would:
                // 1. Generate password reset token
                // 2. Create reset password URL
                // 3. Send email with reset URL
                return RedirectToAction(nameof(ForgotPasswordConfirmation));
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // Helper method to add errors to ModelState
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}