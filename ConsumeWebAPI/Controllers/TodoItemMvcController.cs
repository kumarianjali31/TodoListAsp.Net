using ConsumeWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Permissions;
using System.Text;

namespace ConsumeWebAPI.Controllers
{
    public class TodoItemMvcController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:44305/api");
        private readonly HttpClient _httpClient;
        public TodoItemMvcController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<TodoItemView> list = new List<TodoItemView>();
            HttpResponseMessage httpResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/TodoList/GetToDoItems").Result;
            if (httpResponse.IsSuccessStatusCode)
            {
                string data = httpResponse.Content.ReadAsStringAsync().Result;
                list = JsonConvert.DeserializeObject<List<TodoItemView>>(data);
            }

            return View(list);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(TodoItemView item)
        {
            try
            {
                string data = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = _httpClient.PostAsync(_httpClient.BaseAddress + "/TodoList/PostToDoItem", content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    TempData["successMEssage"] = "Data Added Successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                TodoItemView list = new TodoItemView();
                HttpResponseMessage httpResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/TodoList/GetToDoItem/" + id).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string data = httpResponse.Content.ReadAsStringAsync().Result;
                    list = JsonConvert.DeserializeObject<TodoItemView>(data);
                }

                return View(list);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpPost]
        public ActionResult Edit(TodoItemView item)
        {
            try
            {
                string data = JsonConvert.SerializeObject(item);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage httpResponse = _httpClient.PutAsync(_httpClient.BaseAddress + "/TodoList/PutToDoItem", content).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Data Updated Successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                TodoItemView list = new TodoItemView();
                HttpResponseMessage httpResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/TodoList/GetToDoItem/" + id).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    string data = httpResponse.Content.ReadAsStringAsync().Result;
                    list = JsonConvert.DeserializeObject<TodoItemView>(data);
                }
                return View(list);
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
        }
        [HttpPost,ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "/TodoList/DeleteToDoItem/" + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Data Deleted Successfully";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            return View();
        }
    }
}

