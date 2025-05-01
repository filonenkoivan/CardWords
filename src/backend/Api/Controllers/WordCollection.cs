using Api.Models;
using Api.Models.DTOs;
using Application.Common.Enum;
using Application.Interfaces;
using Application.Models.Response;
using Application.Services;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WordCollection : ControllerBase
    {

        IUserService userService;
        ICollectionService collectionService;
        IQuizService quizService;
        public WordCollection(IUserService _userService, ICollectionService _collectionService, IQuizService _quizService)
        {;
            userService = _userService;
            collectionService = _collectionService;
            quizService = _quizService;
        }

        [Authorize]
        [HttpGet("collections")]
        public async Task<IActionResult> GetCollections(CancellationToken cancellationToken)
        {
            List<CollectionModel> collections = await collectionService.GetCollections(cancellationToken);
            UserStats stats = await userService.GetUserStats(cancellationToken);
            return Ok(new { collections = collections, stats = stats});
        }
        [Authorize]
        [HttpPost("collections")]
        public async Task<IActionResult> CreateCollection(CollectionRequest collection, CancellationToken cancellationToken)
        {
            var result = await collectionService.UpdateCollection(new CardCollection(collection.Name), cancellationToken);

            return result switch
            {
                UpdateCollectionResult.Conflict => Conflict(new {message= "A collection with that name already exists" }),
                UpdateCollectionResult.Success => Ok(),
                _ => StatusCode(500)
            };
        }

        [HttpDelete("collections/{id}")]
        public async Task<IActionResult> DeleteCollection(int id, CancellationToken cancellationToken)
        {
            await collectionService.DeleteCollection(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("collections/{id}")]
        public async Task<IActionResult> GetCollections(int id, CancellationToken cancellationToken)
        {
            CardCollection collectionForSort = await collectionService.GetCollection(id, cancellationToken);

            return Ok(await collectionService.GetCollection(id, cancellationToken));
        }

        [HttpPost("collections/words/{id}")]
        public async Task<IActionResult> AddWordToCollection([FromBody] CardRequest request, int id, CancellationToken cancellationToken)
        {
            Card card = new Card { BackSideText = request.BackSideText, FrontSideText = request.FrontSideText, Decsription = request.Decsription, Priority = request.Priority, CreatedTime = DateTime.UtcNow };
            await collectionService.AddCardToCollecton(card, id, cancellationToken);
            return Ok();
        }

        [HttpDelete("collections/{collectionId}/words/{id}")]
        public async Task<IActionResult> DeleteWord(int collectionId, int id, CancellationToken cancellationToken)
        {
            await collectionService.DeleteWord(collectionId: collectionId, wordId: id, cancellationToken);
            return Ok();
        }


        [HttpGet("quiz/{id}")]
        public async Task<IActionResult> QuizGame(int id, CancellationToken cancellationToken)
        {
            QuizCardModel quizCard = await quizService.CreateTask(id, cancellationToken);
            return Ok(quizCard);
        }

        [HttpGet("updatestats")]
        public async Task<IActionResult> QuizGame(CancellationToken cancellationToken)
        {
            await userService.UpdateUser(cancellationToken);
            return Ok();
        }
    }
}
