using Domain.Models;
using Infrastructure.AppDataContext;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private AppDbContext _db;

       
        public UserRepository(AppDbContext db)
        {
            _db = db; ;
        }


        public async Task AddUser(User user)
        {
            try
            {
                await _db.Users.AddAsync(user);
                await _db.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _db.Users.Include(x => x.Collections).ThenInclude(x => x.CardList).ToListAsync();
        }


        public async Task<User> GetUserByNameAsync(string name)
        {
            return _db.Users.Include(x => x.Collections).ThenInclude(x => x.CardList).FirstOrDefault(x => x.Name == name);
        }

    }
}
