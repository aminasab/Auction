using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineAuction.Interfaces;
using OnlineAuction.Models;
using System.Security.Claims;

namespace OnlineAuction.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly PasswordHasher<UserModel> _passwordHasher;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _passwordHasher = new PasswordHasher<UserModel>();
        }

        [HttpGet]
        public IActionResult Login()
        {
            var model = new LoginViewModel();
            return View(model);
        }

        /// <summary>
        /// Обработка формы входа.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Логика аутентификации
                var result = await AuthenticateUser(model.Email, model.Password);
                if (result)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Неправильное имя пользователя или пароль.");
            }
            return View(model);
        }

        /// <summary>
        /// Отображение страницы регистрации.
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Обработка формы регистрации.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Register(UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                // Проверяем, существует ли уже пользователь с таким именем и фамилией
                var existingUser = await _userRepository.GetUserByFullNameAsync(userModel.FirstName, userModel.LastName);

                // Если пользователь существует, проверяем пароль
                if (existingUser != null)
                {
                    if (_passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, userModel.PasswordHash)
                        != PasswordVerificationResult.Failed)
                    {
                        ModelState.AddModelError("", "Пользователь с таким именем и паролем уже существует.");
                        return View(userModel);
                    }
                }
                // Хэшируем пароль перед сохранением
                userModel.PasswordHash = _passwordHasher.HashPassword(userModel, userModel.PasswordHash);
                await _userRepository.AddUserAsync(userModel);
                return RedirectToAction("Index", "Home");
            }
            return View(userModel);
        }

        public List<Claim> claims = new List<Claim>();

        private async Task<bool> AuthenticateUser(string email, string password)
        {
            var user = await _userRepository.GetUserByFullEmailAsync(email);

            // Если пользователь не найден, возвращаем false
            if (user == null)
            {
                return false;
            }
            claims.Add(new Claim(ClaimTypes.Name, user.Email));
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            var passwordHasher = new PasswordHasher<UserModel>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }
    }
}