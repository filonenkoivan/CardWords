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
    public class QuizService(ICollectionRepository repository, IdentificationService identification) : IQuizService
    {
        Random rand = new Random();
        public async Task<QuizCardModel> CreateTask(int collectionId, CancellationToken cancellationToken)
        {
            User user = await identification.GetUser(cancellationToken);

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

            return new QuizCardModel(answer, shuffledAnswers, correctAnswer);
        }

        private Card GetRandomCard(CardCollection collection)
        {
            int number = collection.CardList.Count;
            Card randomCard = new Card();
            if (number > 0)
            {
                randomCard = collection.CardList.ElementAt(rand.Next(0, number));
                collection.CardList.Remove(randomCard);
            }
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
