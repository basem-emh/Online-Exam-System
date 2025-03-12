using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineExamSystem.BLL.Common.Helper;
using OnlineExamSystem.DAL.Entities.Identity;
using OnlineExamSystem.PL.ViewModels.Identity;

namespace OnlineExamSystem.PL.Controllers
{
    public class AccountController : Controller
    {
        #region Services
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        #endregion
        
        #region Login // Sign In
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
         
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag)
                    {
                      var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                        if (result.Succeeded)
                            return RedirectToAction(nameof(HomeController.Index), "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalid Password");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email Is Not Exsits");
                }

            }
            return View(model);
        }
        #endregion

        #region Logout // Sign Out 
        public async new Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region Reset Password
        #region Forget Password
        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetVM)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgetVM.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    var url = Url.Action("ResetPassword", "Account", new { Email = forgetVM.Email, Token = token }, Request.Scheme);

                    var email = new Email
                    {
                        Body = url,
                        Subject = "Reset Password",
                        To = forgetVM.Email,
                    };
                    EmailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckYourInbox));
                }
            }
            return View(forgetVM);
        }
        #endregion
        #region Check Inbox
        [HttpGet]
        public IActionResult CheckYourInbox()
        {
            return View();
        }
        #endregion

        #region Reset
        [HttpGet]
        public IActionResult ResetPassword(string Email, string Token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

                    if (result.Succeeded)
                        return RedirectToAction(nameof(Login));

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        } 
        #endregion
        #endregion
    }
}
