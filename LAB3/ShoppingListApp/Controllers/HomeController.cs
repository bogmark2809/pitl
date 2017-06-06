using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using ShoppingListApp.Controllers.Classes;
using ShoppingListApp.Models;
using ShoppingListApp.Models.ViewModels;

namespace ShoppingListApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly MainSettings _settings;
        public HomeController(IOptionsSnapshot<MainSettings> settings)
        {
            _settings = settings.Value;
        }
        public async Task<IActionResult> Index()
        {
            string token = User.Identity.Name;
            ShoppingList shoppingLists = HttpContext.Session.Get<ShoppingList>("shoppingLists");
            if (shoppingLists == null)
            {
                using (var client = new HttpClient())
                {
                    try
                    {
                        client.BaseAddress = new Uri(_settings.Url);
                        var response = await client.GetAsync($"{token}");
                        response.EnsureSuccessStatusCode();
        
                        var stringResult = await response.Content.ReadAsStringAsync();
                        shoppingLists = JsonConvert.DeserializeObject<ShoppingList>(stringResult);
                    }
                    catch (Exception)
                    {
                        return View("Error");
                    }
                }
            }
            HttpContext.Session.Set<ShoppingList>("shoppingLists", shoppingLists);
            var listname = HttpContext.Session.Get<string>("MainListName");
            HttpContext.Session.Set("MainListName", string.IsNullOrEmpty(listname) ? shoppingLists.FirstOrDefault().Key : listname);
            BindViewData();
            return View();
        }

        public void BindViewData()
        {
            var listname = HttpContext.Session.Get<string>("MainListName");
            var shoppingLists = HttpContext.Session.Get<ShoppingList>("shoppingLists");
            ViewBag.Products = string.IsNullOrEmpty(listname) ? shoppingLists.FirstOrDefault().Value : shoppingLists[listname];
            ViewData["ListName"] = string.IsNullOrEmpty(listname) ? shoppingLists.FirstOrDefault().Key : listname;
            ViewBag.AllListNames = shoppingLists.Keys.Where(k => k != listname).ToList();
        }

        public IActionResult AddProduct(Product ActionProduct)
        {
            if (ModelState.IsValid)
            {
                var listname = HttpContext.Session.Get<string>("MainListName");
                var shoppingLists = HttpContext.Session.Get<ShoppingList>("shoppingLists");
                if (shoppingLists[listname].Any(p => p.Name.ToLowerInvariant() == ActionProduct.Name.ToLowerInvariant()))
                    ViewData["Error"] = "Product with the same name already exist";
                else
                {
                    shoppingLists[listname].Add(ActionProduct);
                    HttpContext.Session.Set<ShoppingList>("shoppingLists", shoppingLists);
                    return RedirectToAction("Index");
                }
            }
            BindViewData();
            return View("Index", new HomeViewModel(null, ActionProduct));
        }

        public IActionResult DeleteProduct(Product ActionProduct)
        {
            if (ModelState.IsValid)
            {
                var shoppingLists = HttpContext.Session.Get<ShoppingList>("shoppingLists");
                var listname = HttpContext.Session.Get<string>("MainListName");
                shoppingLists[listname].Remove(shoppingLists[listname].First(p => p.Name == ActionProduct.Name && p.Quantity == ActionProduct.Quantity));
                HttpContext.Session.Set<ShoppingList>("shoppingLists", shoppingLists);
                return RedirectToAction("Index");
            }
            BindViewData();
            return View("Index", new HomeViewModel(null, ActionProduct));
        }

        public IActionResult AddList(string ActionListName)
        {
            if (string.IsNullOrEmpty(ActionListName))
                ModelState.AddModelError("ActionListName", "List name field is required");
            if (ModelState.IsValid)
            {
                var shoppingLists = HttpContext.Session.Get<ShoppingList>("shoppingLists");
                if (shoppingLists.Keys.Any(p => p.ToLowerInvariant() == ActionListName.ToLowerInvariant()))
                    ViewData["Error"] = "List with the same name already exist";
                else
                {
                shoppingLists.Add(ActionListName, new List<Product>());
                HttpContext.Session.Set<ShoppingList>("shoppingLists", shoppingLists);
                HttpContext.Session.Set<string>("MainListName", ActionListName);
                return RedirectToAction("Index");
                }
            }
            BindViewData();
            return View("Index", new HomeViewModel(ActionListName, null));
        }

        public IActionResult SelectList(string ActionListName)
        {
            HttpContext.Session.Set<string>("MainListName", ActionListName);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteList(string ActionListName)
        {
            var shoppingLists = HttpContext.Session.Get<ShoppingList>("shoppingLists");
            shoppingLists.Remove(ActionListName);
            HttpContext.Session.Set<ShoppingList>("shoppingLists", shoppingLists);
            HttpContext.Session.Remove("MainListName");
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Sync()
        {
            string token = User.Identity.Name;
            using (var client = new HttpClient())
            {
                try
                {
                    var shoppingList = HttpContext.Session.Get<ShoppingList>("shoppingLists");
                    var requestBody = JsonConvert.SerializeObject(shoppingList);
                    client.BaseAddress = new Uri(_settings.Url);
                    var response = await client.PostAsync($"{token}", new StringContent(requestBody, Encoding.UTF8, "application/json"));
                    response.EnsureSuccessStatusCode();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    return View("Error");
                }
            }
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}