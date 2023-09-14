using System.Text;
using System.Text.Json;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;

namespace Web_153501_Brykulskii.Services.PictureService;

public class ApiPictureService : IPictureService
{
    private readonly HttpClient _httpClient;
    private readonly int _pageSize;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<ApiPictureService> _logger;

    public ApiPictureService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ApiPictureService> logger)
    {
        _httpClient = httpClient;
        _pageSize = configuration.GetValue<int>("ItemsPerPage");
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
    }

    public async Task<ResponseData<Picture>> CreatePictureAsync(Picture picture, IFormFile? formFile)
    {
        var uri = new Uri(_httpClient.BaseAddress!.AbsoluteUri + "Pictures");
        var response = await _httpClient.PostAsJsonAsync(uri, picture, _serializerOptions);
        if (response.IsSuccessStatusCode)
        {
            var data = await response
            .Content
            .ReadFromJsonAsync<ResponseData<Picture>>
            (_serializerOptions);
            return data; // picture;
        }
        _logger.LogError($"-----> object not created. Error:{response.StatusCode}");

        return new ResponseData<Picture>
        {
            Success = false,
            ErrorMessage = $"Объект не добавлен. Error:{response.StatusCode}"
        };
    }

    public Task DeletePictureAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Picture>> GetPictureByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseData<ListModel<Picture>>> GetPictureListAsync(string? genreNormalizedName, int pageNo = 1)
    {
        // подготовка URL запроса
        var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}pictures/");
        // добавить категорию в маршрут
        if (genreNormalizedName != null)
        {
            urlString.Append($"{genreNormalizedName}/");
        };
        // добавить номер страницы в маршрут
        if (pageNo > 1)
        {
            urlString.Append($"page{pageNo}");
        };
        // добавить размер страницы в строку запроса
        if (!_pageSize.Equals("3"))
        {
            urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));
        }
        // отправить запрос к API
        var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

        if (response.IsSuccessStatusCode)
        {
            try
            {
                return await response
                .Content
                .ReadFromJsonAsync<ResponseData<ListModel<Picture>>>
                (_serializerOptions);
            }
            catch (JsonException ex)
            {
                _logger.LogError($"-----> Ошибка: {ex.Message}");
                return new ResponseData<ListModel<Picture>>
                {
                    Success = false,
                    ErrorMessage = $"Ошибка: {ex.Message}"
                };
            }
        }

        _logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
        return new ResponseData<ListModel<Picture>>
        {
            Success = false,
            ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
        };
    }

    public Task UpdatePictureAsync(int id, Picture picture, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }
}
