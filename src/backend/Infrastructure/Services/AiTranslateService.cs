using Application.Common.Exceptions;
using Application.Interfaces;
using Application.Models.Responsel;
using Domain.Enum;
using Infrastructure.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.Mime.MediaTypeNames;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Services;

public class AiTranslateService : IAiTranslateService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IOptions<AiConfiguration> _options;
    public AiTranslateService(IHttpClientFactory httpClientFactory, IOptions<AiConfiguration> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = options;
    }
    public async Task<TranslationDTO> GetTranslationArray(string word)
    {
        string prompt =
            "You are a professional translator. Your task is to translate the given word into English or Ukrainian, depending on the input language." +
            "Provide exactly 5 possible translations or closest equivalents, ranked by relevance." +
            "Return only the words/ phrases, separated by commas, without additional text." +
            "Example: Input: 'книга' → Output: book, books, novel, publication, literature." +
            $"The word is: {word}";

        var client = _httpClientFactory.CreateClient("ai-translate");
        client.DefaultRequestHeaders.Add("Authorization", "Bearer " + _options.Value.ApiKey);
        string url = _options.Value.ApiUrl;


        var request = new
        {
            model = _options.Value.Model,
            messages = new[] {

                new {
                        role = "user",
                        content = prompt
                    }
            }
        };

        JsonContent content = JsonContent.Create(request);


        var result = await client.PostAsync(url, content);

        if (!result.IsSuccessStatusCode)
        {
            throw new HttpTranslatorException($"Error: {result.StatusCode}, {await result.Content.ReadAsStringAsync()}");
        }

        var newResult = await result.Content.ReadAsStringAsync();

        JObject responseJson = JObject.Parse(newResult);
        string translationList = (string)responseJson["choices"][0]["message"]["content"];
        if (translationList == null)
        {
            throw new HttpTranslatorException("Translation not found in the response.");
        }
        var translationArray = translationList.Trim(' ').Split(",");

        var translationFinalList = new List<string>(translationArray.Select(t => t.Trim()));
        var translations = new TranslationDTO(translationFinalList);

        return translations;
    }
}

