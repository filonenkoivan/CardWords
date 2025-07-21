using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models.Responsel;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Services;

public class ApiTranslationService : IApiTranslationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<ApiTranslationConfiguration> _options;
    public ApiTranslationService(IHttpClientFactory httpClientFactory, IOptions<ApiTranslationConfiguration> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options;
    }
    public async Task<TranslationDTO> GetTranslationArray(string word)
    {
        var translations = new List<string>();
        var client = _httpClientFactory.CreateClient("api-translate");


        var response = await client.GetAsync(_options.Value.GoogleTranslate + word);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpTranslatorException($"Error: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}");
        }

        var stringResponse = await response.Content.ReadAsStringAsync();
        if (stringResponse == null)
        {
            throw new HttpTranslatorException("Translation not found in the response.");
        }
        string[][] responseJson = JsonSerializer.Deserialize<string[][]>(stringResponse);
        translations.Add(responseJson[0][0]);

        return new TranslationDTO(translations);
    }
}

