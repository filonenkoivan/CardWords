using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    //[Route("[controller]")]
    [ApiController]
    public class DefaultRouteController : ControllerBase
    {
        //[Authorize]
        //[HttpGet("/profile.html")]
        //public async Task Index()
        //{
        //    var html = await File.ReadAllTextAsync("wwwroot/profile.html"); // не в wwwroot!
        //    HttpContext.Response.ContentType = "text/html";
        //    await HttpContext.Response.WriteAsync(html);
        //}

        ////[Authorize]
        ////[HttpGet("/profile.html")]
        ////public IActionResult CollectionPage() { 

        ////}
        //[HttpGet("/login")]
        //public IActionResult Login()
        //{
        //    return View;
        //}

    }
}
