using Vapie.WebUI.AppCode.Extensions;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities.Membership;
using Vapie.WebUI.Models.FormModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Vapie.WebUI.Controllers
{
    
    public class AccountController : Controller
    {
        private readonly VapieDbContext db;
        private readonly SignInManager<VapieUser> signInManager;
        private readonly UserManager<VapieUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IActionContextAccessor ctx;

        public AccountController(VapieDbContext db, SignInManager<VapieUser> signInManager, UserManager<VapieUser> userManager,
            IConfiguration configuration,
            IActionContextAccessor ctx)
        {
            this.db = db;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.ctx = ctx;
        }
        [Route("/signin.html")]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }
        [Route("/profile.html")]
        public IActionResult Profile()
        {
            return View();
        }
        [Route("/accessdenied.html")]
        public IActionResult AccessDenied()
        {
            return View();
        }
        [Route("/logout.html")]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }
        [HttpPost]
        [Route("/signin.html")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginFormModel user)
        {
            if (ModelState.IsValid)
            {
                VapieUser foundedUser = null;

                if (user.UserName.IsEmail())
                {
                    foundedUser = await userManager.FindByEmailAsync(user.UserName);
                }
                else
                {
                    foundedUser = await userManager.FindByNameAsync(user.UserName);
                }


                if (foundedUser == null)
                {
                    ViewBag.Message = "E-Poçtunuz və ya şifrəniz yanlışdır!";
                    goto end;
                }
                var signInResult = await signInManager.PasswordSignInAsync(foundedUser, user.Password, true, true);

                if (!signInResult.Succeeded)
                {
                    ViewBag.Message = "E-Poçtunuz və ya şifrəniz yanlışdır!";
                    goto end;
                }

                var callBackUrl = Request.Query["ReturnUrl"];

                if (!string.IsNullOrWhiteSpace(callBackUrl))
                {
                    return Redirect(callBackUrl);
                }
                return RedirectToAction("Index", "Home");
            }

            end:
            return View(user);
        }
        [AllowAnonymous]
        [Route("/register.html")]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [Route("/register.html")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new VapieUser();
                user.Email = model.Email;
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.UserName = $"{model.Name}.{model.Surname}";

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string path = $"{ctx.GetAppLink()}/registration-confirm.html?email={user.Email}&token={token}";
                    var emailResponse = configuration.SendEmail(user.Email, "Vapie istifadəçi qeydiyyatı", $"Zəhmət olmasa <a href=\"{path}\">link</a> vasitəsilə qeydiyyatı tamamlayasınız");
                    if (emailResponse)
                    {
                        ViewBag.Message = "Təbriklər qeydiyyat Tamamlandı, Sizinlə Tezliklə Əlaqə Saxlanılacaq";
                    }
                    else
                    {
                        ViewBag.Message = "E-mailə göndərərkən səhv aşkar olundu, yenidən cəhd edin";
                    }

                    return RedirectToAction(nameof(SignIn));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        [Route("/registration-confirm.html")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterConfirm(string email, string token)
        {
            var foundedUser = await userManager.FindByEmailAsync(email);
            if (foundedUser == null)
            {
                ViewBag.Message = "Xətalı Token göndərilib";
                goto end;
            }
            token = token.Replace(" ", "+");
            var result = await userManager.ConfirmEmailAsync(foundedUser, token);
            if (!result.Succeeded)
            {
                ViewBag.Message = "Xətalı Token göndərilib";
                goto end;
            }
            ViewBag.Message = "Hesabınız təsdiqləndi, sizinlə tezliklə əlaqə saxlanılacaq";
        end:
            return RedirectToAction(nameof(SignIn));
        }
    }
}
