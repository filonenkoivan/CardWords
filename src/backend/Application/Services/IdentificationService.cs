using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class IdentificationService
    {
        IHttpContextAccessor accessor;
        IUserService userService;

        public IdentificationService(IHttpContextAccessor _accessor, IUserService _userService)
        {
            accessor = _accessor;
            userService = _userService;
        }

        public async Task<User> GetUser(CancellationToken cancellationToken)
        {

            string userName = accessor?.HttpContext?.User?.Identity?.Name;
            User currentUser = await userService.GetUserByNameAsync(userName, cancellationToken);
            return currentUser;
        }
    }
}
