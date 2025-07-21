using Api.Models.DTOs;
using Application.Common.Enum;
using Application.Interfaces;
using Domain.Enum;
using Domain.Models;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CollectionService : ICollectionService
    {
        ICollectionRepository _repository;
        IdentificationService identificationService;
        UserStatsService statsService;


        public CollectionService(ICollectionRepository repository, IdentificationService _identificationService, UserStatsService _statsService)
        {
            _repository = repository;
            identificationService = _identificationService;
            statsService = _statsService;
        }
        public async Task<UpdateCollectionResult> UpdateCollection(CardCollection list, CancellationToken cancellationToken)
        {
            User currentUser = await identificationService.GetUser(cancellationToken);

            if (currentUser.Collections.Any(x => x.Name == list.Name))
            {
                return UpdateCollectionResult.Conflict;

            }

            await _repository.UpdateCollection(currentUser, list, cancellationToken);

            return UpdateCollectionResult.Success;
        }
        public async Task AddCardToCollecton(Card card, int id, CancellationToken cancellationToken)
        {
            User currentUser = await identificationService.GetUser(cancellationToken);

            currentUser.Stats = await statsService.StatsUpdate(isAdd: true, cancellationToken: cancellationToken, stats: currentUser.Stats);

            await _repository.AddCard(currentUser, card, id, cancellationToken);
        }
        public async Task<CardCollection> GetCollection(int id, CancellationToken cancellationToken)
        {
            return await _repository.GetCollection(id, cancellationToken);
        }
        public async Task DeleteCollection(int id, CancellationToken cancellationToken)
        {
            User user = await identificationService.GetUser(cancellationToken);
            await _repository.Delete(user, id, cancellationToken);
        }
        public async Task<List<CollectionModel>> GetCollections(CancellationToken cancellationToken)
        {
            User user = await identificationService.GetUser(cancellationToken);
            return _repository.GetCollections(user, cancellationToken);
        }

        public async Task DeleteWord(int collectionId, int wordId, CancellationToken cancellationToken)
        {
            User currentUser = await identificationService.GetUser(cancellationToken);

            await _repository.DeleteWord(currentUser, collectionId, wordId, cancellationToken);
        }
    }
}
