using Api.Models;
using Application.Interfaces;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        IPasswordHasherService hasher;
        IUserService service;
        public AuthController(IPasswordHasherService _hasher, IUserService _service)
        {
            hasher = _hasher;
            service = _service;
        }
        //додати норм авторизацію
        //authorize


        //[Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRequest user)
        {
            var currentUser = await service.GetUserByNameAsync(user.Name);

            if (currentUser != null && hasher.HashVerify(user.Password, currentUser.Password))
            {
                var Claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Name) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(Claims, "Cookies");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Ok(new {success = true});
            }
            return Unauthorized(new {success = false, message = "Invalid credentials"});
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/index.html");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest user)
        {
            Console.WriteLine("Hello world!");
            var allUsers = await service.GetUsers();
            if (allUsers.Any(x => x.Name == user.Name))
            {
                return BadRequest(new {message = "User already exists" });
            }
            service.AddUser(new User { Name = user.Name, Password = hasher.HashPassword(user.Password) });
            return Ok(new {message = "New user created"});
        }
    }
}
