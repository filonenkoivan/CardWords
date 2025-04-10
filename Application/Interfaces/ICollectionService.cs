using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICollectionService
    {
        Task UpdateCollection(User user, CardCollection list);
        Task DeleteCollection(User user, int id);
        Task<CardCollection> GetCollection(User user, int id);
        public Task AddCardToCollecton(User user, Card card, int id);
    }
}
