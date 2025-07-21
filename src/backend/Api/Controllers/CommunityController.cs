using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CommunityController : ControllerBase
    {
        private readonly ICommunityService _communityService;
        public CommunityController(ICommunityService communityService)
        {
            _communityService = communityService;
        }

        [HttpGet("collections")]
        public async Task<IActionResult> GetCollections(string name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("You need to provide the name");
            }
            var result = await _communityService.GetCollections(name, cancellationToken);
            return Ok(result);
        }

        [HttpGet("collections/save")]
        public async Task<IActionResult> SaveCollection(int id, CancellationToken cancellationToken)
        {
            if (id == null)
            {
                return BadRequest("You need to provide the id");
            }
            var result = await _communityService.SaveCollection(id, cancellationToken);
            if (result == false)
            {
                return NotFound("Collection not found or already saved");
            }

            return Ok(result);
        }
    }
}
