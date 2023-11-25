using Web_153501_Brykulskii.API.Data;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;

namespace Web_153501_Brykulskii.API.Services;

public class PictureGenreService : IPictureGenreService
{
	private readonly AppDbContext _context;

	public PictureGenreService(AppDbContext context)
	{
		_context = context;
	}

	public Task<ResponseData<List<PictureGenre>>> GetPictureGenreListAsync()
	{
		var genres = _context.Genres.ToList();

		if (genres == null)
		{
			return Task.FromResult(new ResponseData<List<PictureGenre>>
			{
				Success = false,
				ErrorMessage = "Genres not found"
			});
		}

		var result = new ResponseData<List<PictureGenre>>
		{
			Data = genres,
			Success = true,
		};

		return Task.FromResult(result);
	}
}
