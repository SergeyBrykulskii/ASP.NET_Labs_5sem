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
		ILogger<ApiPictureService> logger)
	{
		_httpClient = httpClient;
		_serializerOptions = new JsonSerializerOptions()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};
		_logger = logger;
	}
	public async Task<ResponseData<List<PictureGenre>>> GetPictureGenreListAsync()
	{
		var urlString = $"{_httpClient.BaseAddress!.AbsoluteUri}PictureGenres/";

		var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

		if (response.IsSuccessStatusCode)
		{
			try
			{
				return await response
				.Content
				.ReadFromJsonAsync<ResponseData<List<PictureGenre>>>
				(_serializerOptions);
			}
			catch (JsonException ex)
			{
				_logger.LogError($"-----> Ошибка: {ex.Message}");
				return new ResponseData<List<PictureGenre>>
				{
					Success = false,
					ErrorMessage = $"Ошибка: {ex.Message}"
				};
			}
		}

		_logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
		return new ResponseData<List<PictureGenre>>
		{
			Success = false,
			ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
		};
	}
}
