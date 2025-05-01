using Api.Models.DTOs;
using Application.Common.Enum;
using Domain.Enum;
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
        Task<UpdateCollectionResult> UpdateCollection(CardCollection list, CancellationToken cancellationToken);
        Task DeleteCollection(int id, CancellationToken cancellationToken);
        Task<CardCollection> GetCollection(int id, CancellationToken cancellationToken);
        Task AddCardToCollecton(Card card, int id, CancellationToken cancellationToken);
        Task<List<CollectionModel>> GetCollections(CancellationToken cancellationToken);
        Task DeleteWord(int collectionId, int wordId, CancellationToken cancellationToken);
    }
}
