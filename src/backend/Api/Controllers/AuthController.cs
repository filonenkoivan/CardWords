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

using System.ComponentModel.DataAnnotations;


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
        UserStatsService statsService;
        public AuthController(IPasswordHasher _hasher, IUserService _service, JwtProvider _jwtService, IValidator<UserRequest> _validator, UserStatsService _statsService)
        {
            hasher = _hasher;
            service = _service;
            jwtProvider = _jwtService;
            validator = _validator;
            statsService = _statsService;
        }

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
            return Unauthorized(new {success = false, message = "Incorrect username or password. Please try again." });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Response.Cookies.Delete("crumble-cookies");;
            return NoContent();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRequest user, CancellationToken cancellationToken, IValidator<UserRequest> _validator)
        {

            var validationResult = await validator.ValidateAsync(user);

            if (!validationResult.IsValid)
            {

                var errors = validationResult.Errors.
                    Select(x => new { Field = x.PropertyName, Message = x.ErrorMessage});
                foreach (var item in errors)
                {
                    Console.WriteLine(item.Field);
                    Console.WriteLine(item.Message);
                }
                return BadRequest(errors);
            }

            var currentUser = await service.GetUserByNameAsync(user.Name, cancellationToken, false);

            if (currentUser != null)
            {
                return Conflict(new {Message = "User already exists" });
            }

            User newUser = new User()
            {
                Name = user.Name,
                Password = hasher.HashPassword(user.Password),
                Stats = new UserStats()
                {
                    Name = user.Name
                }

            };
            await service.AddUser(newUser, cancellationToken);
            return Ok(new {message = "New user created"});
        }
    }
}
