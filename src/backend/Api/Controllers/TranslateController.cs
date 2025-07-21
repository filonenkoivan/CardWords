using Application.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("[controller]")]
[ApiController]
public class TranslateController : ControllerBase
{
    private readonly IAiTranslateService _translateService;
    private readonly IApiTranslationService _apiTranslateService;
    public TranslateController(IAiTranslateService translateService, IApiTranslationService apiTranslateService)
    {
        _translateService = translateService;
        _apiTranslateService = apiTranslateService;
    }
    [HttpGet()]
    public async Task<IActionResult> GetTransalateList(string word)
    {
        //var translations = await _translateService.GetTranslationArray(word);
        var translations = await _apiTranslateService.GetTranslationArray(word);

        return Ok(translations);
    }
}

