using Api.Models;
using Application.Interfaces;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Application.Services;
using Infrastructure.Providers;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        IPasswordHasher hasher;
        IUserService service;
        JwtProvider jwtProvider;
        public AuthController(IPasswordHasher _hasher, IUserService _service, JwtProvider _jwtService)
        {
            hasher = _hasher;
            service = _service;
            jwtProvider = _jwtService;
        }



        //[Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRequest user, CancellationToken cancellationToken)
        {
            var currentUser = await service.GetUserByNameAsync(user.Name, cancellationToken);

            if (currentUser != null && hasher.HashVerify(user.Password, currentUser.Password))
            {
                string token = jwtProvider.GenerateToken(currentUser);
                HttpContext.Response.Cookies.Append("crumble-cookies", token);

                return Ok(new {succes = true});
            }
            return Unauthorized(new {success = false, message = "Invalid credentials"});
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("crumble-cookies");;
            return Redirect("/login.html");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest user, CancellationToken cancellationToken)
        {
            Console.WriteLine("Hello world!");
            var allUsers = await service.GetUsers(cancellationToken);
            if (allUsers.Any(x => x.Name == user.Name))
            {
                return BadRequest(new {message = "User already exists" });
            }
            await service.AddUser(new User { Name = user.Name, Password = hasher.HashPassword(user.Password) }, cancellationToken);
            return Ok(new {message = "New user created"});
        }
    }
}
