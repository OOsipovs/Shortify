using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortify.Client.Data.ViewModels;
using Shortify.Client.Helpers.Roles;
using Shortify.Data;
using Shortify.Data.Models;
using Shortify.Data.Services;

namespace Shortify.Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUsersService  usersService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;

        public AuthController(IUsersService usersService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            this.usersService = usersService;
            this.signInManager = signInManager;
            this.userManager = userManager; 
        }

        public async Task<IActionResult> Users()
        {
            var users = await usersService.GetUsersAsync();

            return View(users);
        }

        public async Task<IActionResult> Login()
        {
            return View(new LoginVM());
        }

        public async Task<IActionResult> Register()
        {
            return View(new RegisterVM());
        }

        public async Task<IActionResult> LoginSubmission(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Login", loginVM);
            }

            var user = await userManager.FindByEmailAsync(loginVM.Email);
            if(user != null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var userLoggedIn = await signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);

                    if (userLoggedIn.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    } 
                    else
                    {
                        ModelState.AddModelError("", "Ivalid login attempt.");
                        return View("Login", loginVM);
                    }
                } 
                else
                {
                    await userManager.AccessFailedAsync(user);
                    if(await userManager.IsLockedOutAsync(user))
                    {
                        ModelState.AddModelError("", "Your account is locked please try again in 10 miinutes");
                        return View("Login", loginVM);
                    }

                    ModelState.AddModelError("", "YIvalid login attempt.");
                    return View("Login", loginVM);
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> RegisterUser(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Register", registerVM);
            }

            var existingUser = await userManager.FindByEmailAsync(registerVM.Email);
            if(existingUser != null)
            {
                ModelState.AddModelError("", "Email address is already in use.");
                return View("Register", registerVM);
            }
              
            var newUser = new AppUser()
            {
                Email = registerVM.Email,
                UserName = registerVM.Email,
                FullName = registerVM.FullName,
                LockoutEnabled = true,
            };

            var userCreated = await userManager.CreateAsync(newUser, registerVM.Password);

            if (userCreated.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, Role.User);
                await signInManager.PasswordSignInAsync(newUser, registerVM.Password, false, false);
            }
            else
            {
                foreach (var error in userCreated.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("Register", registerVM);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
