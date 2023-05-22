using APIConsumeAndCrud.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http.Headers;

namespace APIConsumeAndCrud.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        string baseURL = "http://localhost:5182/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Calling WEB API and Inserting it in View using Entity Model Class
            IList<TodoEntity> todo = new List<TodoEntity>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("todos");
                if (getData.IsSuccessStatusCode)
                {
                    string results = getData.Content.ReadAsStringAsync().Result;
                    todo = JsonConvert.DeserializeObject<List<TodoEntity>>(results);
                }
                else
                {
                    Console.WriteLine("Error Calling Web Api");
                }

                ViewData.Model = todo;
            }

            return View();
        }

        [HttpGet]
        public async Task<ActionResult<String>> AddTodo(TodoEntity todo)
        {
            TodoEntity obj = new TodoEntity()
            {
                Id = Guid.NewGuid(),
                Title = todo.Title,
                Description = todo.Description,
                isComplete = false,

            };
            if (todo.Title != null)
            {

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage getData = await client.PostAsJsonAsync<TodoEntity>("todos", obj);
                    Console.WriteLine(getData.ToString());
                    if (getData.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        Console.WriteLine("Error Calling Web Api");
                    }

                }

            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditTodo(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL + "todos/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.GetAsync($"{id}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var todo = JsonConvert.DeserializeObject<TodoEntity>(content);
                    return View("EditTodo", todo);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTodo(TodoEntity todo)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL + "todos/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = await client.PutAsJsonAsync($"{todo.Id}", todo);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage deleteData = await client.DeleteAsync($"todos/{id}");
                Console.WriteLine(deleteData.ToString());
                if (deleteData.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    Console.WriteLine("Error Calling Web Api");
                }
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}