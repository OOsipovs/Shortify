using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;
using Shortify.Client.Data.ViewModels;
using Shortify.Client.Helpers.Roles;
using Shortify.Data;
using Shortify.Data.Models;
using Shortify.Data.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Shortify.Client.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUsersService  usersService;
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly IConfiguration configuration;

        public AuthController(IUsersService usersService, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration)
        {
            this.usersService = usersService;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;

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
                    else if (userLoggedIn.IsNotAllowed)
                    {
                        return RedirectToAction("EmailConfirmation");
                    }
                    else if (userLoggedIn.RequiresTwoFactor)
                    {
                        return RedirectToAction("TwoFactor", new { loggedInUserId = user.Id}); 
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

        public async Task<IActionResult> EmailConfirmation()
        {
            var confirmEmail = new ConfirmEmailLoginVM();
            return View(confirmEmail);
        }

        public async Task<IActionResult> SendEmailConfirmation(ConfirmEmailLoginVM confirmEmailLogin)
        {
            var user = await userManager.FindByEmailAsync(confirmEmailLogin.Email);
            
            if (user != null)
            {
                var userToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var userConfirmationLink = Url.Action("EmailConfirmationVerified", "Auth", new
                {
                    userId = user.Id,
                    userConfirmationToken = userToken
                }, Request.Scheme);

                //Send email using sendgrid
                var apiKey = configuration["SendGrid:ShortlyKey"];
                var sendGridClient = new SendGridClient(apiKey);

                var fromEmailAddress = new EmailAddress(configuration["SendGrid:FromAddress"], "Shortify Client App");
                var emailSubject = "[Shortify] Verify your account";
                var toEmailAddress = new EmailAddress(confirmEmailLogin.Email, "Shortify Client App");

                var emailContentTxt = "Hello from Shortify app, please click this link to verify your acount " + userConfirmationLink;
                var emailContentHtml = $"Hello from Shortify app, please click this link to verify your acount: <a href=\"{userConfirmationLink}\"> Verify your account </a>" ;

                var emailRequest = MailHelper.CreateSingleEmail(fromEmailAddress, toEmailAddress, emailSubject, emailContentTxt, emailContentHtml);
                var emailResponse = new Response(System.Net.HttpStatusCode.NotFound, null, null);//sendGridClient.SendEmailAsync(emailRequest);
                TempData["EmailConfirmation"] = "Thank you! Please, check you emailto verify your account";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", $"Email address {confirmEmailLogin.Email} does not exist");
            return View("EmailConfirmation", confirmEmailLogin);

        }

        public async Task<IActionResult> EmailConfirmationVerified(string userId, string userConfirmationToken)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await userManager.ConfirmEmailAsync(user,userConfirmationToken);
            TempData["EmailConfirmationVerified"] = "Thank you! Your account has been confirmed";
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> TwoFactor(string loggedInUserId)
        {
            var user = await userManager.FindByIdAsync(loggedInUserId);
            if (user != null)
            {
                var userToken = await userManager.GenerateTwoFactorTokenAsync(user, "Phone");
                string twilioPhoneNumber = configuration["Twilio:PhoneNumber"];
                string twilioSID = configuration["Twilio:SID"];
                string twilioToken = configuration["Twilio:Token"];

                TwilioClient.Init(twilioSID, twilioToken);

                var msg = MessageResource.Create(
                    body: $"This is your verification code: {userToken}",
                    from: new Twilio.Types.PhoneNumber(twilioPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(user.PhoneNumber));


                var confirm2FALoginVM = new Confirm2FALoginVM()
                {
                    UserId = loggedInUserId
                };

                return View(confirm2FALoginVM); 
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> TwoFactorVerified(Confirm2FALoginVM confirm2FALoginVM)
        {
            var user = await userManager.FindByIdAsync(confirm2FALoginVM.UserId);
            if(user != null)
            {
                var tokenVerification = await userManager.VerifyTwoFactorTokenAsync(user, "Phone", confirm2FALoginVM.UserConfirmationCode);

                if (tokenVerification)
                {
                    var tokenSignIn = await signInManager.TwoFactorSignInAsync("Phone", confirm2FALoginVM.UserConfirmationCode, false, false);
                    if (tokenSignIn.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Confirmation code is not corrrect");

            return View(confirm2FALoginVM);
        }
    }
}
