using Application.Interfaces;
using Domain.Models;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CollectionService : ICollectionService
    {
        ICollectionRepository _repository;
        public CollectionService(ICollectionRepository repository)
        {
            _repository = repository;
        }
        public async Task UpdateCollection(User user, CardCollection list, CancellationToken cancellationToken)
        {
            await _repository.UpdateCollection(user, list, cancellationToken);
        }

        public async Task AddCardToCollecton(User user, Card card, int id, CancellationToken cancellationToken)
        {
            await _repository.AddCard(user, card, id, cancellationToken);
        }

        public async Task<CardCollection> GetCollection(User user, int id, CancellationToken cancellationToken)
        {
            return await _repository.GetCollection(user, id, cancellationToken);
        }
        public async Task DeleteCollection(User user, int id, CancellationToken cancellationToken)
        {
            await _repository.Delete(user, id, cancellationToken);
        }
    }
}
