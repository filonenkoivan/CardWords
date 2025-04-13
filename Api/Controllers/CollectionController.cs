using Api.Models;
using Api.Models.DTOs;
using Application.Interfaces;
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
    public class CollectionController : ControllerBase
    {

        IUserService userService;
        ICollectionService collectionService;
        ICollectionSortService sortService;
        public CollectionController(IUserService _userService, ICollectionService _collectionService, ICollectionSortService _sortService)
        {;
            userService = _userService;
            collectionService = _collectionService;
            sortService = _sortService;
        }

        [Authorize]
        [HttpGet("collections")]
        public async Task<IActionResult> GetCollections(CancellationToken cancellationToken)
        {
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName, cancellationToken);

            var list = currentUser?.Collections.Select(x => new CollectionDTO(name: x.Name, cardList: x.CardList, createdTime: x.CreatedTime, id: x.Id));

            return Ok(new { collections = list, name = HttpContext.User.Identity?.Name });
        }
        [Authorize]
        [HttpPost("collections")]
        public async Task<IActionResult> CreateCollection(CollectionRequest collection, CancellationToken cancellationToken)
        {
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName, cancellationToken);


            if (currentUser.Collections.Any(x => x.Name == collection.Name))
            {
                return Conflict(new { message = "This item already exists!" });

            }
            await collectionService.UpdateCollection(currentUser, new CardCollection { Name = collection.Name, CreatedTime = DateTime.UtcNow }, cancellationToken);

            var newCollection = currentUser.Collections.Select(x => new CollectionDTO(name: x.Name, cardList: x.CardList, createdTime: x.CreatedTime, id: x.Id)).FirstOrDefault(x => x.Name == collection.Name);

            return Ok(new { message = "item added", item = newCollection });
        }

        [HttpDelete("collections/{id}")]
        public async Task<IActionResult> DeleteCollection(int id, CancellationToken cancellationToken)
        {
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName, cancellationToken);
            await collectionService.DeleteCollection(currentUser, id, cancellationToken);
            return Ok();
        }

        [HttpGet("collections/{id}/{sort=list}")]
        public async Task<IActionResult> GetCollections(int id, string sort, CancellationToken cancellationToken)
        {
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName, cancellationToken);
            CardCollection collectionForSort = await collectionService.GetCollection(currentUser, id, cancellationToken);
            if (sort == "list")
            {
                return Ok(await collectionService.GetCollection(currentUser, id, cancellationToken));

            }
            return Ok(await sortService.SortForPlay(collectionForSort));

            //оцей результат треба буде повернути
            //отрмиати колекцію, прокрутити через сортувальений сервіс і повернути
        }

        [HttpPatch("collections/{id}")]
        public async Task<IActionResult> UpdateCollection(int id, Card card, CancellationToken cancellationToken)
        {
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName, cancellationToken);
            currentUser?.Collections?.FirstOrDefault(x => x.Id == id)?.CardList?.Add(card);
            return Ok();
        }


        [HttpPost("words/{id}")]
        public async Task<IActionResult> AddWordToCollection([FromBody] CardRequest request, int id, CancellationToken cancellationToken)
        {

            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName, cancellationToken);

            //var collection = currentUser?.Collections?.FirstOrDefault(x => x.Id == id);
            var card = new Card { BackSideText = request.BackSideText, FrontSideText = request.FrontSideText, Decsription = request.Decsription, Priority = request.Priority, CreatedTime = DateTime.UtcNow };

            await collectionService.AddCardToCollecton(currentUser, card, id, cancellationToken);
            return Ok();
        }

        [HttpPatch("words/{id}")]
        public async Task<IActionResult> UpdateWord([FromBody] Card card, int id, CancellationToken cancellationToken)
        {
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName, cancellationToken);

            var oldWord = currentUser.Collections.FirstOrDefault(x => x.Id == id).CardList.FirstOrDefault(x => x.Id == card.Id);
            oldWord.Priority = card.Priority;
            oldWord.ExpiresTime = await sortService.ExpiresDate(card);
            return Ok();
            // Дописати алгортим, карточки повинні появлятись якщо їх мало
            // пофіксити баг де якщо слово одне рандомайзер в грі не виводить нічого!
        }

    }
}
