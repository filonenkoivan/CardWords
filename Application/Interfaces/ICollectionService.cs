using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICollectionService
    {
        Task UpdateCollection(User user, CardCollection list, CancellationToken cancellationToken);
        Task DeleteCollection(User user, int id, CancellationToken cancellationToken);
        Task<CardCollection> GetCollection(User user, int id, CancellationToken cancellationToken);
        public Task AddCardToCollecton(User user, Card card, int id, CancellationToken cancellationToken);
    }
}
