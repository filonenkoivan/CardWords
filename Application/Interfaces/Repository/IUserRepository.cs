using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task AddUser(User user, CancellationToken cancellationToken);

        Task<User> GetUserByNameAsync(string name, CancellationToken cancellationToken, bool includeAllData = true);
    }
}
