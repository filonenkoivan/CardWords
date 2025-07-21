using Api.Models.DTOs;
using Application.Interfaces.Repository;
using Dapper;
using Domain.Models;
using Infrastructure.AppDataContext;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories;

//maybe rewrite this class with dapper
public class CommunityRepository : ICommunityRepository
{
    private readonly AppDbContext _db;
    public CommunityRepository(AppDbContext db)
    {
        _db = db;
    }
    public async Task<List<CollectionModel>> GetCollectionsAsync(string name, int userId, CancellationToken ct)
    {

        var collections = _db.CardCollections.Include(x => x.CardList.Take(5)).Where(x => EF.Functions.ILike(x.Name, $"%{name}%") && x.UserId != userId);

        var collectionModels = await collections.Select(x => x.ToModel()).ToListAsync(ct);

        return collectionModels;
    }

    public async Task<bool> SaveCollection(int id, int userId, CancellationToken ct)
    {
        var user = await _db.Users.FirstOrDefaultAsync(x => x.Id == userId, ct);
        if (user.Collections.Any(x => x.Id == id))
        {
            return false;
        }

        var userCollection = await _db.CardCollections.Include(x => x.CardList).FirstOrDefaultAsync(x => x.Id == id, ct);
        if (userCollection == null)
        {
            return false;
        }



        var savedWords = userCollection.CardList.Select(x => new Card
        {
            FrontSideText = x.FrontSideText,
            BackSideText = x.BackSideText,
            Decsription = x.Decsription,
            CreatedTime = x.CreatedTime
        }).ToList();

        var savedCollection = new CardCollection(userCollection.Name)
        {
            CardList = savedWords
        };

        user.Collections.Add(savedCollection);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}

