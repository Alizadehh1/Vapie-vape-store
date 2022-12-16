using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Extensions;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities.Membership;
using Vapie.WebUI.Models.FormModels;

namespace Vapie.WebUI.Controllers
{

    public class AccountController : Controller
    {
        private readonly VapieDbContext db;
        private readonly SignInManager<VapieUser> signInManager;
        private readonly UserManager<VapieUser> userManager;
        private readonly IConfiguration configuration;
        private readonly IActionContextAccessor ctx;
        private readonly IMediator mediator;

        public AccountController(VapieDbContext db, SignInManager<VapieUser> signInManager, UserManager<VapieUser> userManager,
            IConfiguration configuration,
            IActionContextAccessor ctx,
            IMediator mediator)
        {
            this.db = db;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.configuration = configuration;
            this.ctx = ctx;
            this.mediator = mediator;
        }
        [Route("/signin.html")]
        [AllowAnonymous]
        public IActionResult SignIn()
        {
            return View();
        }
        [Route("/profile.html")]
        public async Task<IActionResult> Profile()
        {
            var user = await userManager.FindByIdAsync(User.GetUserId());
            return View(user);
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
        //[Route("/changePassword.html")]
        //public IActionResult ChangePassword()
        //{

        //    return View();
        //}
        //[HttpPost("/changePassword.html")]
        //public async Task<IActionResult> ChangePassword(ChangePasswordFormModel model)
        //{
            
        //    if (ModelState.IsValid)
        //    {
        //        var userId = User.GetUserId();
        //        var user = await userManager.FindByIdAsync(userId);
        //        var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        //        if (result.Succeeded)
        //        {
        //            ViewBag.IsSuccess = true;
        //            ModelState.Clear();
        //            return View();
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //    }

        //    return View(model);
        //}
        [HttpPost]
        [Route("/signin.html")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(LoginFormModel user)
        {
            if (ModelState.IsValid)
            {
                VapieUser foundedUser = null;

                foundedUser = await userManager.FindByEmailAsync(user.Email);


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
                if (db.Users.Any(u => u.Email == model.Email))
                {
                    ViewBag.Message = "Bu email ilə artıq qeydiyyatdan keçmisiniz";
                    goto end;
                }
                user.UserName = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.PhoneNumberConfirmed = true;
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    string path = $"{ctx.GetAppLink()}/registration-confirm.html?email={user.Email}&token={token}";
                    var emailResponse = configuration.SendEmail(user.Email, "Vapie istifadəçi qeydiyyatı", $"Zəhmət olmasa <a href=\"{path}\">link</a> vasitəsilə qeydiyyatı tamamlayasınız");
                    if (emailResponse)
                    {
                        ViewBag.Message = "Təbriklər qeydiyyat tamamlandı, zəhmət olmasa elektron poçtunuza gələn mail'i təsdiqləyəsiniz. ";
                    }
                    else
                    {
                        ViewBag.Message = "E-mailə göndərərkən səhv aşkar olundu, yenidən cəhd edin";
                    }

                    return View();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
        end:
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
            ViewBag.Message = "Hesabınız təsdiqləndi";
        end:
            return RedirectToAction(nameof(SignIn));
        }
    }
}
