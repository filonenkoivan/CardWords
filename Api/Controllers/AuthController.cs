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
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        IPasswordHasher hasher;
        IUserService service;
        JwtProvider jwtProvider;
        IValidator<UserRequest> validator;
        public AuthController(IPasswordHasher _hasher, IUserService _service, JwtProvider _jwtService, IValidator<UserRequest> _validator)
        {
            hasher = _hasher;
            service = _service;
            jwtProvider = _jwtService;
            validator = _validator;
        }



        //[Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserRequest user, CancellationToken cancellationToken)
        {
            var currentUser = await service.GetUserByNameAsync(user.Name, cancellationToken, false);
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
            // Iactionresult
            var validationResult = await validator.ValidateAsync(user);
            //Console.WriteLine(validationResult);

            if (!validationResult.IsValid)
            {

                var errors = validationResult.Errors.
                    Select(x => new { Field = x.PropertyName, Message = x.ErrorMessage});

                return BadRequest(errors);
            }


            // доробити валідацію
            var currentUser = await service.GetUserByNameAsync(user.Name, cancellationToken, false);
            if (currentUser != null)
            {
                return Conflict(new {Message = "User already exists" });
            }
            await service.AddUser(new User { Name = user.Name, Password = hasher.HashPassword(user.Password) }, cancellationToken);
            return Ok(new {message = "New user created"});
        }
    }
}
