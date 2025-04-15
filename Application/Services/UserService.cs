using Application.Interfaces;
using Domain.Models;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {

        private readonly IUserRepository _repository;
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }
        public async Task AddUser(User user, CancellationToken cancellationToken)
        {
            await _repository.AddUser(user, cancellationToken);
        }


        public async Task<List<User>> GetUsers(CancellationToken cancellationToken)
        {
            return await _repository.GetAllUsers(cancellationToken);
        }


        public Task<User> GetUserByNameAsync(string name, CancellationToken cancellationToken, bool includeAllData = true)
        {
            return _repository.GetUserByNameAsync(name, cancellationToken, includeAllData);
        }
    }
}
