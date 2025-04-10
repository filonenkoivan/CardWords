using Application.Interfaces;
using Domain.Enum;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CollectionSortService : ICollectionSortService
    {

        public async Task<DateTime> ExpiresDate(Card card)
        {
            DateTime expiresTime = DateTime.UtcNow;
            switch (card.Priority)
            {
                case CardPriority.Learned:  
                expiresTime = DateTime.UtcNow.AddDays(3);
                break;
                case CardPriority.Easy:
                expiresTime = DateTime.UtcNow.AddDays(2);
                break;
                case CardPriority.Medium:
                expiresTime = DateTime.UtcNow.AddDays(1);
                break;
                case CardPriority.Hard:
                expiresTime = DateTime.UtcNow.AddHours(5);
                break;
            }

            return expiresTime;
        }

        public async Task<CardCollection> SortForPlay(CardCollection collection)
        {
            var newCollection = collection.CardList.Where(x => x.ExpiresTime <= DateTime.UtcNow).ToList();
          
            collection.CardList = newCollection;

            return collection;
            //сортує і видає лише ті елементи які ми типу не вивчили ще
        }
    }
}
