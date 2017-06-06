using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using ShoppingListApp.Models.ViewModels;

namespace ShoppingListApp.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ActionName("Login")]
        public async Task<IActionResult> LoginConfirm(LoginViewModel loginModel)
        {
            string token = loginModel.Token;
            if (string.IsNullOrEmpty(loginModel.Token))
                token = Guid.NewGuid().ToString();
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, token)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "AuthTokenCookie", ClaimsIdentity.DefaultNameClaimType, 
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.Authentication.SignInAsync("AuthCookie", new ClaimsPrincipal(id));
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logoff()
        {
            HttpContext.Authentication.SignOutAsync("AuthCookie");
            return View("Login");
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}