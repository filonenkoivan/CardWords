﻿using Domain.Models;
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

        public async Task UpdateCollection(User user, CardCollection list, CancellationToken cancellationToken = default)
        {
            var currentUser = await _db.Users
                .Include(x => x.Collections)
                .ThenInclude(x => x.CardList)
                .FirstOrDefaultAsync(x => x.Id == user.Id);
            currentUser.Collections.Add(list);
            await _db.SaveChangesAsync(cancellationToken);
        }
        public async Task<CardCollection> GetCollection(User user, int id, CancellationToken cancellationToken = default)
        {
            var collectionUser = await _db.Users
                .Include(x => x.Collections)
                .FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken: cancellationToken);

            if (collectionUser == null) return null;
                
            return collectionUser.Collections.FirstOrDefault(x => x.Id == id);
        }

        public async Task Delete(User user, int id, CancellationToken cancellationToken = default)
        {
            var collectionUser = await _db.Users
                .Include(u => u.Collections)
                .FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken: cancellationToken);

            if (collectionUser == null) return;

            var collectionToRemove = collectionUser.Collections.FirstOrDefault(x => x.Id == id);
            if (collectionToRemove == null) return;

            collectionUser.Collections.Remove(collectionToRemove);

            await _db.SaveChangesAsync(cancellationToken);
        }
        public async Task AddCard(User user, Card card, int id, CancellationToken cancellationToken = default)
        {

            var collectionUser = await _db.Users
                .Include(x => x.Collections)
                .ThenInclude(x => x.CardList)
                .FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);

            if (collectionUser == null) return;

            var collection = collectionUser?.Collections?.FirstOrDefault(x => x.Id == id);
            if (collection == null) return;

            collection?.CardList?.Add(card);

            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
