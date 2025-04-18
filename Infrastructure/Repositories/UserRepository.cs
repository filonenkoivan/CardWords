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
            try{
                await _db.Users.AddAsync(user, cancellationToken: cancellationToken);
                await _db.SaveChangesAsync(cancellationToken: cancellationToken);
            }
            catch(OperationCanceledException e)
            {
                Console.WriteLine($"error: {e}, action canceled");
            }

        }


        public async Task<User> GetUserByNameAsync(string name, CancellationToken cancellationToken = default, bool includeAllData = true)
        {

            if (includeAllData == false)
            {
                return await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
                //if error check asnotracking method
            }

            return await _db.Users
                .Include(x => x.Collections)
                .ThenInclude(x => x.CardList)
                .FirstOrDefaultAsync(x => x.Name == name, cancellationToken: cancellationToken);
        }

    }
}
