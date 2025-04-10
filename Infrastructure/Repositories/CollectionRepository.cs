using Domain.Models;
using Infrastructure.AppDataContext;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CollectionRepository : ICollectionRepository
    {
        AppDbContext _db;
        public CollectionRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task UpdateCollection(User user, CardCollection list)
        {
            var currentUser = await _db.Users.Include(x => x.Collections).ThenInclude(x => x.CardList).FirstOrDefaultAsync(x => x.Id == user.Id);
            currentUser.Collections.Add(list);
            await _db.SaveChangesAsync();
        }
        public async Task<CardCollection> GetCollection(User user, int id)
        {
            return _db.Users.FirstOrDefault(x => x.Id == user.Id).Collections.FirstOrDefault(x => x.Id == id);
        }

        public async Task Delete(User user, int id)
        {

            _db.Users.FirstOrDefault(x => x.Id == user.Id).Collections.FirstOrDefault(x => x.Id == id);
            _db.Users.FirstOrDefault(x => x.Id == user.Id).Collections.Remove(_db.Users.FirstOrDefault(x => x.Id == user.Id).Collections.FirstOrDefault(x => x.Id == id));

            await _db.SaveChangesAsync();
        }
        public async Task AddCard(User user, Card card, int id)
        {
            user.Collections.FirstOrDefault(x => x.Id == id).CardList.Add(card);
            await _db.SaveChangesAsync();
        }
    }
}
