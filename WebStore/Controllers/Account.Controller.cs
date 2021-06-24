using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.Entitys.Identity;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        
        #region Регистрация
        [AllowAnonymous]
        public IActionResult Register() => View(new RegisterUserViewModel());

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUserViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            var user = new User
            {
                UserName = Model.UserName
            };

            var register_result = await _userManager.CreateAsync(user, Model.Password);

            if (register_result.Succeeded)
            {
                //await _userManager.AddToRoleAsync(user, Role.Users);
                await _userManager.AddToRoleAsync(user, Role.Users);
                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var identity_error in register_result.Errors)
            {
                ModelState.AddModelError("", identity_error.Description);
            }

            return View(Model);
        }
        #endregion

        [AllowAnonymous]
        public IActionResult Login(string ReturnUrl) => 
            View(new LoginViewModel { ReturnUrl = ReturnUrl });
        
        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel Model)
        {
            if (!ModelState.IsValid) return View(Model);

            var login_result = await _signInManager.PasswordSignInAsync(
                Model.UserName, Model.Password, Model.Remember,
#if DEBUG
                false
#else
                true
#endif
                );
            
            if (login_result.Succeeded)
            {
                //return Redirect(Model.ReturnUrl); НЕ БЕЗОПАСНО

                //if (Url.IsLocalUrl(Model.ReturnUrl))
                //    return Redirect(Model.ReturnUrl);
                //else
                //    return RedirectToAction("Index", "Home");

                return LocalRedirect(Model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Ошибка логина, или пароля");
            return View(Model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        public IActionResult AccessDenied() => View();
    }
}
