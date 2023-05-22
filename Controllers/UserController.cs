using APIConsumeAndCrud.Models;
using Microsoft.AspNetCore.Mvc;
namespace ConsumeWebApi.Controllers
{
    public class UserController : Controller
    {
        private string baseURL = "http://localhost:5182/";
        public async Task<IActionResult> Login(UserEntity user)
        {
            UserEntity obj = new UserEntity
            {
                Email = user.Email,
                Password = user.Password
            };
            if (obj.Email != null && obj.Password != null)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseURL + "User/");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync($"?Email={obj.Email}&Password={obj.Password}");
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
            return View();
        }
    }
}