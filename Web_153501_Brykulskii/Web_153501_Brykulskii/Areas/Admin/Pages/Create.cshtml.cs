using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Services.PictureGenreService;
using Web_153501_Brykulskii.Services.PictureService;

namespace Web_153501_Brykulskii.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IPictureService _pictureService;
        private readonly IPictureGenreService _pictureGenreService;

        public CreateModel(
            IPictureService pictureService,
            IPictureGenreService pictureGenreService)
        {
            _pictureService = pictureService;
            _pictureGenreService = pictureGenreService;
        }

        public async Task<IActionResult> OnGet()
        {
            var response = await _pictureGenreService.GetPictureGenreListAsync();

            if (!response.Success)
            {
                return NotFound(response.ErrorMessage);
            }

            ViewData["GenreId"] = new SelectList(response.Data, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Picture Picture { get; set; } = default!;

        [BindProperty]
        public IFormFile? Image { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var response = await _pictureService.CreatePictureAsync(Picture, Image);

            if (!response.Success)
                return NotFound(response.ErrorMessage);

            return RedirectToPage("./Index");
        }
    }
}
