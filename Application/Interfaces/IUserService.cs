using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task AddUser(User user);
        Task<List<User>> GetUsers();

        Task<User> GetUserByNameAsync(string name);

    }
}
