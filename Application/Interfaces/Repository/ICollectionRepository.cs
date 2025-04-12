using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface ICollectionRepository
    {
        Task UpdateCollection(User user, CardCollection list);

        Task Delete(User user, int id);

        Task AddCard(User user, Card card, int id);

        Task<CardCollection> GetCollection(User user, int id);
    }
}
