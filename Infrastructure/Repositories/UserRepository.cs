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
using System.Threading;
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


        public async Task AddUser(User user, CancellationToken cancellationToken = default)
        {
            await _db.Users.AddAsync(user, cancellationToken: cancellationToken);
            await _db.SaveChangesAsync(cancellationToken: cancellationToken);
        }

        public async Task<List<User>> GetAllUsers(CancellationToken cancellationToken = default)
        {
            return await _db.Users
                .Include(x => x.Collections)
                .ThenInclude(x => x.CardList).ToListAsync(cancellationToken);
        }


        public async Task<User> GetUserByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await _db.Users
                .Include(x => x.Collections)
                .ThenInclude(x => x.CardList)
                .FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);
        }

    }
}
