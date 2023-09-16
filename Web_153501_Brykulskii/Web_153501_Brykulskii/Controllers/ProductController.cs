using Microsoft.AspNetCore.Mvc;
using Web_153501_Brykulskii.Converters;
using Web_153501_Brykulskii.Services.PictureGenreService;
using Web_153501_Brykulskii.Services.PictureService;

namespace Web_153501_Brykulskii.Controllers;

public class ProductController : Controller
{
    private readonly IPictureGenreService _pictureGenreService;
    private readonly IPictureService _pictureService;

    public ProductController(
        IPictureGenreService pictureGenreService,
        IPictureService pictureService)
    {
        _pictureGenreService = pictureGenreService;
        _pictureService = pictureService;
    }
    public async Task<IActionResult> Index(string? genre, int pageNo = 1)
    {
        var genresResponse = await _pictureGenreService.GetPictureGenreListAsync();
        var pictureResponse = await _pictureService.GetPictureListAsync(genre, pageNo);

        if (!pictureResponse.Success || !genresResponse.Success)
        {
            return NotFound(pictureResponse.ErrorMessage + '\n' + genresResponse.ErrorMessage);
        }

        ViewData["genres"] = genresResponse.Data;
        ViewData["currentGenre"] = GenreConverter.ConvertToRu(genre);

        return View((pictureResponse.Data!.Items, pictureResponse.Data.CurrentPage, pictureResponse.Data.TotalPages));
    }
}
