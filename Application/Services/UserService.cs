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
        public async Task AddUser(User user)
        {
            await _repository.AddUser(user);
        }


        public async Task<List<User>> GetUsers()
        {
            return await _repository.GetAllUsers();
        }


        public Task<User> GetUserByNameAsync(string name)
        {
            return _repository.GetUserByNameAsync(name);
        }
    }
}
