using Application.Models.Response;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IQuizService
    {
        public Task<QuizCardModel> CreateTask(int collectionId, CancellationToken cancellationToken);
    }
}
