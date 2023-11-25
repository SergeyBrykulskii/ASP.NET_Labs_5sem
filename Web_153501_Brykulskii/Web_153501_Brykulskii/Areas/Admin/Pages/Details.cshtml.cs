using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Services.PictureGenreService;
using Web_153501_Brykulskii.Services.PictureService;

namespace Web_153501_Brykulskii.Areas.Admin.Pages
{
	public class DetailsModel : PageModel
	{
		private readonly IPictureService _pictureService;
		private readonly IPictureGenreService _pictureGenreService;

		public DetailsModel(
			IPictureService pictureService,
			IPictureGenreService pictureGenreService)
		{
			_pictureService = pictureService;
			_pictureGenreService = pictureGenreService;
		}

		public Picture Picture { get; set; } = default!;
		public PictureGenre Genre { get; set; } = default!;

		public async Task<IActionResult> OnGetAsync(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var responsePicture = await _pictureService.GetPictureByIdAsync(id.Value);
			var responseGenres = await _pictureGenreService.GetPictureGenreListAsync();

			if (!responsePicture.Success || !responseGenres.Success)
			{
				return NotFound(responsePicture.ErrorMessage + '\n' + responseGenres.ErrorMessage);
			}

			Picture = responsePicture.Data!;
			Genre = responseGenres.Data!.FirstOrDefault(g => g.Id == Picture.GenreId);

			return Page();
		}
	}
}
