using Microsoft.AspNetCore.Mvc;
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
        ViewData["genres"] = (await _pictureGenreService.GetPictureGenreListAsync()).Data;

        var pictureResponse = await _pictureService.GetPictureListAsync(genre, pageNo);

        if (!pictureResponse.Success)
        {
            return NotFound(pictureResponse.ErrorMessage);
        }

        ViewData["currentGenre"] = genre == null ? genre : pictureResponse.Data?.Items?.FirstOrDefault()?.Genre?.Name;

        return View((pictureResponse.Data!.Items, pictureResponse.Data.CurrentPage, pictureResponse.Data.TotalPages));
    }
}
