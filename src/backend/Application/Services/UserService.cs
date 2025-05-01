using Application.Interfaces;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        IHttpContextAccessor accessor;
        private readonly IUserRepository _repository;
        UserStatsService statsService;
        public UserService(IUserRepository repository, IHttpContextAccessor _accessor, UserStatsService _statsService)
        {
            _repository = repository;
            accessor = _accessor;
            statsService = _statsService;
        }
        public async Task AddUser(User user, CancellationToken cancellationToken)
        {
            await _repository.AddUser(user, cancellationToken);
        }

        public async Task<User> GetUserByNameAsync(string name, CancellationToken cancellationToken, bool includeAllData = true)
        {
            return await _repository.GetUserByNameAsync(name, cancellationToken, includeAllData);
        }

        public async Task<UserStats> GetUserStats(CancellationToken cancellationToken)
        {
            string userName = accessor?.HttpContext?.User?.Identity?.Name;

            User user = await GetUserByNameAsync(userName, cancellationToken);

            return await _repository.GetUserStats(user, cancellationToken);

        }

        public async Task UpdateUser(CancellationToken cancellationToken)
        {
            string userName = accessor?.HttpContext?.User?.Identity?.Name;

            User user = await GetUserByNameAsync(userName, cancellationToken);
            user.Stats = await statsService.StatsUpdate(isAdd: false, cancellationToken, stats: user.Stats);

            await _repository.UpdateUser(user, cancellationToken);
        }

    }
}
