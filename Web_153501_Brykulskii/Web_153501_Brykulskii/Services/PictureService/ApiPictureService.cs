using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
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
	private readonly HttpContext _httpContext;

	public ApiPictureService(
		HttpClient httpClient,
		IConfiguration configuration,
		ILogger<ApiPictureService> logger,
		IHttpContextAccessor httpContextAccessor)
	{
		_httpClient = httpClient;
		_pageSize = configuration.GetValue<int>("ItemsPerPage");
		_serializerOptions = new JsonSerializerOptions()
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase
		};
		_logger = logger;
		_httpContext = httpContextAccessor.HttpContext!;
	}

	public async Task<ResponseData<Picture>> CreatePictureAsync(Picture picture, IFormFile? formFile)
	{
		var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Pictures");

		var token = await _httpContext.GetTokenAsync("access_token");
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

		var response = await _httpClient.PostAsJsonAsync(uri, picture, _serializerOptions);

		if (response.IsSuccessStatusCode)
		{
			var data = await response
				.Content
				.ReadFromJsonAsync<ResponseData<Picture>>
				(_serializerOptions);

			if (formFile != null)
			{
				await SaveImageAsync(data.Data.Id, formFile);
			}

			return data; // picture;
		}

		_logger.LogError($"-----> object not created. Error:{response.StatusCode}");

		return new ResponseData<Picture>
		{
			Success = false,
			ErrorMessage = $"Объект не добавлен. Error:{response.StatusCode}"
		};
	}

	public async Task<ResponseData<ListModel<Picture>>> GetPictureListAsync(string? genreNormalizedName, int pageNo = 1)
	{
		var urlString = new StringBuilder($"{_httpClient.BaseAddress!.AbsoluteUri}Pictures/");

		var token = await _httpContext.GetTokenAsync("access_token");
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

		if (genreNormalizedName != null)
			urlString.Append($"{genreNormalizedName}/");

		if (pageNo > 1)
			urlString.Append($"page{pageNo}");

		if (!_pageSize.Equals("3"))
			urlString.Append(QueryString.Create("pageSize", _pageSize.ToString()));

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

	public async Task DeletePictureAsync(int id)
	{
		var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Pictures/{id}");

		var token = await _httpContext.GetTokenAsync("access_token");
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

		var response = await _httpClient.DeleteAsync(uri);

		if (!response.IsSuccessStatusCode)
		{
			_logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
		}
	}

	public async Task<ResponseData<Picture>> GetPictureByIdAsync(int id)
	{
		var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Pictures/{id}");

		var token = await _httpContext.GetTokenAsync("access_token");
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

		var response = await _httpClient.GetAsync(uri);

		if (response.IsSuccessStatusCode)
		{
			try
			{
				return await response
					.Content
					.ReadFromJsonAsync<ResponseData<Picture>>
					(_serializerOptions);
			}
			catch (JsonException ex)
			{
				_logger.LogError($"-----> Ошибка: {ex.Message}");
				return new ResponseData<Picture>
				{
					Success = false,
					ErrorMessage = $"Ошибка: {ex.Message}"
				};
			}
		}

		_logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
		return new ResponseData<Picture>
		{
			Success = false,
			ErrorMessage = $"Данные не получены от сервера. Error: {response.StatusCode}"
		};
	}

	public async Task UpdatePictureAsync(int id, Picture picture, IFormFile? formFile)
	{
		var uri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Pictures/{id}");

		var token = await _httpContext.GetTokenAsync("access_token");
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

		var response = await _httpClient.PutAsync(uri, new StringContent(JsonSerializer.Serialize(picture), Encoding.UTF8, "application/json"));

		if (response.IsSuccessStatusCode)
		{
			if (formFile != null)
				await SaveImageAsync(id, formFile);
		}
		else
		{
			_logger.LogError($"-----> Данные не получены от сервера. Error:{response.StatusCode}");
		}
	}
	private async Task SaveImageAsync(int id, IFormFile image)
	{
		var request = new HttpRequestMessage
		{
			Method = HttpMethod.Post,
			RequestUri = new Uri($"{_httpClient.BaseAddress!.AbsoluteUri}Dishes/{id}")
		};

		var token = await _httpContext.GetTokenAsync("access_token");
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

		var content = new MultipartFormDataContent();
		var streamContent = new StreamContent(image.OpenReadStream());
		content.Add(streamContent, "formFile", image.FileName);
		request.Content = content;

		await _httpClient.SendAsync(request);
	}
}
