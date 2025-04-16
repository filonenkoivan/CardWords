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
        Task AddCard(User user, Card card, int id, CancellationToken cancellationToken);

        Task UpdateCollection(User user, CardCollection list, CancellationToken cancellationToken);

        Task Delete(User user, int id, CancellationToken cancellationToken);


        Task<CardCollection> GetCollection(User user, int id, CancellationToken cancellationToken);
    }
}
