using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;

namespace Web_153501_Brykulskii.Services.PictureGenreService;

public class MemoryPictureGenreService : IPictureGenreService
{
	public Task<ResponseData<List<PictureGenre>>> GetPictureGenreListAsync()
	{
		var genres = new List<PictureGenre>
		{
			new PictureGenre { Id=1, Name="Портрет", NormalizedName="portrait" },
			new PictureGenre { Id=2, Name="Пейзаж", NormalizedName="landscape" },
			new PictureGenre { Id=3, Name="Марина", NormalizedName="marina" },
			new PictureGenre { Id=4, Name="Натюрморт", NormalizedName="still-life" },

		};

		var result = new ResponseData<List<PictureGenre>>
		{
			Data = genres,
			Success = true,
		};

		return Task.FromResult(result);
	}
}
