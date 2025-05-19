using ConsumeWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ConsumeWebAPI.Controllers
{
    public class AccountController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44305/api");
        private readonly HttpClient _httpClient;

        public AccountController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginView model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync("/api/Auth/Login", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    // In real app, handle JWT/cookie here
                    TempData["successMessage"] = "Login successful";
                    return RedirectToAction("Index", "TodoItemMvc");
                }
                else
                {
                    TempData["errorMessage"] = "Invalid credentials";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterView model)
        {
            try
            {
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = _httpClient.PostAsync("/api/Auth/Register", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Registration successful. Please log in.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["errorMessage"] = "Registration failed.";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }
            return View(model);
        }
    }
}
