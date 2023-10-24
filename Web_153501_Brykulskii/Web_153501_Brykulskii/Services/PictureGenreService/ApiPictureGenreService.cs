using System.Text.Json;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;
using Web_153501_Brykulskii.Services.PictureService;

namespace Web_153501_Brykulskii.Services.PictureGenreService;

public class ApiPictureGenreService : IPictureGenreService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _serializerOptions;
    private readonly ILogger<ApiPictureService> _logger;

    public ApiPictureGenreService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<ApiPictureService> logger)
    {
        _httpClient = httpClient;
        _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        _logger = logger;
    }
    public Task<ResponseData<List<PictureGenre>>> GetPictureGenreListAsync()
    {
        throw new NotImplementedException();
    }
}
