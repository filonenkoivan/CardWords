using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        Task AddUser(User user, CancellationToken cancellationToken);

        Task<User> GetUserByNameAsync(string name, CancellationToken cancellationToken, bool includeAllData = true);

        Task<UserStats> GetUserStats(CancellationToken cancellationToken);

        Task UpdateUser(CancellationToken cancellationToken);
    }
}
