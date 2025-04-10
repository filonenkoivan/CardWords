using Domain.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICollectionSortService
    {
        Task<CardCollection> SortForPlay(CardCollection collection);
        Task<DateTime> ExpiresDate(Card card);
    }


}
