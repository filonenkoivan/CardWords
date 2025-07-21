using Application.Configuration;
using Domain.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Providers
{
    public class JwtProvider(IOptions<AuthSettings> options)
    {
        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("userId", user.Id.ToString())
            };
            var jwtToken = new JwtSecurityToken(
                expires: DateTime.UtcNow.AddHours(options.Value.Expires),
                claims: claims,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SecretKey)), SecurityAlgorithms.HmacSha256 )
            );
            Console.WriteLine(DateTime.UtcNow);
            Console.WriteLine(options.Value.Expires);

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
