using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace RaabbitMQWeb.ExcelCreate.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> Login(string Email,string Password)
        {
            var hasUser = await _userManager.FindByEmailAsync(Email);
            if (hasUser == null)
            {
                return View();
            }

            // byte[] d = Convert.FromBase64String(hasUser.PasswordHash);
            //var singInResult = await _signInManager.PasswordSignInAsync(hasUser.Email, Password, true, false);
            byte[] byt = System.Text.Encoding.UTF8.GetBytes(Password);
          var  strModified = Convert.ToBase64String(byt);
            var singInResult = await _signInManager.PasswordSignInAsync(hasUser, Password.ToString(), true, false);
            if (!singInResult.Succeeded)
            {//sorun var
                return View();
            }

            return RedirectToAction(nameof(HomeController.Index),"Home");
        }
    }
}
