using Application.Interfaces;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            var hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
            return hashedPassword;
        }

        public bool HashVerify(string password, string hashedPassword)
        {

            var verifyPassword = BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);

            if (verifyPassword)
            {
                return true;
            }
            return false;
        }
    }
}
