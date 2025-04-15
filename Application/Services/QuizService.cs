using Application.Interfaces;
using Application.Models.Response;
using Domain.Models;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class QuizService(ICollectionRepository repository) : IQuizService
    {
        Random rand = new Random();
        public async Task<QuizCard> CreateTask(int collectionId, User user, CancellationToken cancellationToken)
        {
            var collection = await repository.GetCollection(user, collectionId, cancellationToken);

            Card randomCard = GetRandomCard(collection);

            string correctAnswer = randomCard.BackSideText;
            string answer = randomCard.FrontSideText;
            List<string> answers = new List<string>();


            while(answers.Count < 2)
            {
                var randomCardForAnswer = GetRandomCard(collection).BackSideText;

                if (randomCardForAnswer != randomCard.BackSideText && !answers.Contains(randomCardForAnswer))
                {
                    answers.Add(randomCardForAnswer);
                }
            }
            answers.Add(correctAnswer);

            var shuffledAnswers = Shuffle(answers);

            return new QuizCard(answer, shuffledAnswers, correctAnswer);
        }

        private Card GetRandomCard(CardCollection collection)
        {
            Card randomCard = collection.CardList.ElementAt(rand.Next(0, collection.CardList.Count));
            return randomCard;
        }

        private List<string> Shuffle(List<string> list)
        {
            int count = list.Count;
            while (count > 1)
            {
                count--;
                int k = rand.Next(count + 1);
                var value = list[k];

                list[k] = list[count];
                list[count] = value;
            }

            return list;
        }

    }
}
