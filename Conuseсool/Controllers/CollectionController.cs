using Api.Models;
using Api.Models.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Models;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        IUserRepository repository;
        IPasswordHasherService hasher;
        IUserService userService;
        ICollectionService collectionService;
        ICollectionSortService sortService;
        public CollectionController(IUserRepository _repository, IPasswordHasherService _hasher, IUserService _userService, ICollectionService _collectionService, ICollectionSortService _sortService)
        {
            repository = _repository;
            hasher = _hasher;
            userService = _userService;
            collectionService = _collectionService;
            sortService = _sortService;
        }


        [HttpGet("collection")]
        public async Task<IActionResult> GetCollections()
        {
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName);

            var list = currentUser?.Collections.Select(x => new CollectionDTO(name: x.Name, cardList: x.CardList, createdTime: x.CreatedTime, id: x.Id));

            return Ok(new { collections = list, name = HttpContext.User.Identity?.Name });
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreateCollection(CollectionRequest collection)
        {
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName);
            if (currentUser.Collections.Any(x => x.Name == collection.Name))
            {
                return BadRequest(new { message = "This item already exists!" });

            }
            await collectionService.UpdateCollection(currentUser, new CardCollection { Name = collection.Name, CreatedTime = DateTime.UtcNow });

            var newCollection = currentUser.Collections.Select(x => new CollectionDTO(name: x.Name, cardList: x.CardList, createdTime: x.CreatedTime, id: x.Id)).FirstOrDefault(x => x.Name == collection.Name);

            return Ok(new { message = "item added", item = newCollection });
        }

        [HttpDelete("collection/{id}")]
        public async Task<IActionResult> DeleteCollection(int id)
        {
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName);
            collectionService.DeleteCollection(currentUser, id);
            return Ok();
        }

        [HttpGet("collection/{id}/{sort=list}")]
        public async Task<IActionResult> GetCollections(int id, string sort)
        {
            Console.WriteLine("IMHERE");
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName);
            CardCollection collectionForSort = await collectionService.GetCollection(currentUser, id);
            if (sort == "list")
            {
                return Ok(await collectionService.GetCollection(currentUser, id));

            }
            return Ok(await sortService.SortForPlay(collectionForSort));

            //оцей результат треба буде повернути
            //отрмиати колекцію, прокрутити через сортувальений сервіс і повернути
        }

        [HttpPatch("collection/{id}")]
        public async Task<IActionResult> UpdateColectin(int id, Card card)
        {
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName);
            currentUser.Collections.FirstOrDefault(x => x.Id == id).CardList.Add(card);
            return Ok();
        }


        [HttpPost("words/{id}")]
        public async Task<IActionResult> AddWordToCollection([FromBody] CardRequest request, int id)
        {
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName);

            var collection = currentUser?.Collections?.FirstOrDefault(x => x.Id == id);
            var card = new Card { BackSideText = request.BackSideText, FrontSideText = request.FrontSideText, Decsription = request.Decsription, Priority = request.Priority, CreatedTime = DateTime.UtcNow };

            collectionService.AddCardToCollecton(currentUser, card, id);
            return Ok();
        }

        [HttpPatch("words/{id}")]
        public async Task<IActionResult> UpdateWord([FromBody] Card card, int id)
        {
            var userName = HttpContext.User.Identity?.Name;
            var currentUser = await userService.GetUserByNameAsync(userName);

            var oldWord = currentUser.Collections.FirstOrDefault(x => x.Id == id).CardList.FirstOrDefault(x => x.Id == card.Id);
            oldWord.Priority = card.Priority;
            oldWord.ExpiresTime = await sortService.ExpiresDate(card);
            return Ok();
            // Дописати алгортим, карточки повинні появлятись якщо їх мало
            // пофіксити баг де якщо слово одне рандомайзер в грі не виводить нічого!
        }

    }
}
