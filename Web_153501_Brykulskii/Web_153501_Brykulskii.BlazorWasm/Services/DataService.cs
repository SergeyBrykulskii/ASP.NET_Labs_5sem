using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;

namespace Web_153501_Brykulskii.BlazorWasm.Services;

public class DataService : IDataService
{
    private readonly HttpClient _httpClient;
    private readonly int _pageSize;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly IAccessTokenProvider _accessTokenProvider;

    public List<PictureGenre>? Genres { get; set; }
    public List<Picture>? Pictures { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }

    public event Action? OnPicturesChange;

    public DataService(HttpClient httpClient, IConfiguration configuration, IAccessTokenProvider accessTokenProvider)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetValue<int>("PageSize");
        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        _accessTokenProvider = accessTokenProvider;
    }

    public async Task GetGenreListAsync()
    {
        var tokenResult = await _accessTokenProvider.RequestAccessToken();
        if (tokenResult.TryGetToken(out var token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
        }
        var uri = _httpClient.BaseAddress?.AbsoluteUri + "genres/";
        var response = _httpClient.GetAsync(uri);

        if (response.Result.IsSuccessStatusCode)
        {
            try
            {
                var genres = await response.Result.Content.ReadFromJsonAsync<List<PictureGenre>>(_jsonSerializerOptions);
                Genres = genres;
                Success = true;
            }
            catch (JsonException ex)
            {
                ErrorMessage = $"Что-то пошло не так при запросе жанров. Ошибка: {ex.Message}";
                Success = false;
            }
        }
        else
        {
            ErrorMessage = "Что-то пошло не так при запросе жанров.";
            Success = false;
        }
    }

    public async Task<Picture?> GetPictureByIdAsync(int id)
    {
        var tokenResult = await _accessTokenProvider.RequestAccessToken();
        if (tokenResult.TryGetToken(out var token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
        }
        var uri = _httpClient.BaseAddress?.AbsoluteUri + $"pictures/{id}";
        var response = _httpClient.GetAsync(uri);

        if (response.Result.IsSuccessStatusCode)
        {
            try
            {
                Success = true;
                return (await response.Result.Content.ReadFromJsonAsync<ResponseData<Picture>>(_jsonSerializerOptions))?.Data;
            }
            catch (JsonException ex)
            {
                ErrorMessage = $"Что-то пошло не так при запросе картинки. Ошибка: {ex.Message}";
                Success = false;
                return null;
            }
        }
        else
        {
            ErrorMessage = "Что-то пошло не так при запросе картинки.";
            Success = false;
            return null;
        }
    }

    public async Task GetPicturesListAsync(string? categoryNormalizedName, int pageNo = 1)
    {
        var tokenResult = await _accessTokenProvider.RequestAccessToken();
        if (tokenResult.TryGetToken(out var token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
        }
        var uri = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}pictures/");

        if (categoryNormalizedName != null)
            uri.Append($"{categoryNormalizedName}/");

        if (pageNo > 1)
            uri.Append($"page{pageNo}");

        if (!_pageSize.Equals("3"))
            uri.Append(QueryString.Create("pageSize", _pageSize.ToString()));

        var response = await _httpClient.GetAsync(uri.ToString());

        if (response.IsSuccessStatusCode)
        {
            try
            {
                var responseData = await response.Content.ReadFromJsonAsync<ResponseData<ListModel<Picture>>>(_jsonSerializerOptions);
                Pictures = responseData?.Data?.Items;
                TotalPages = responseData?.Data?.TotalPages ?? 0;
                CurrentPage = responseData?.Data?.CurrentPage ?? 0;
                Success = true;
                OnPicturesChange?.Invoke();
            }
            catch (JsonException ex)
            {
                ErrorMessage = $"Что-то пошло не так при запросе картинок. Ошибка: {ex.Message}";
                Success = false;
            }
        }
        else
        {
            ErrorMessage = "Что-то пошло не так при запросе картинок.";
            Success = false;
        }
    }
}
