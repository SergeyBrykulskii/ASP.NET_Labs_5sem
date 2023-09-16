using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Services.PictureGenreService;
using Web_153501_Brykulskii.Services.PictureService;

namespace Web_153501_Brykulskii.Areas.Admin.Pages
{
    public class EditModel : PageModel
    {
        private readonly IPictureService _pictureService;
        private readonly IPictureGenreService _pictureGenreService;

        public EditModel(
            IPictureService pictureService,
            IPictureGenreService pictureGenreService)
        {
            _pictureService = pictureService;
            _pictureGenreService = pictureGenreService;
        }

        [BindProperty]
        public Picture Picture { get; set; } = default!;

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

            ViewData["GenreId"] = new SelectList(responseGenres.Data!, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _pictureService.UpdatePictureAsync(Picture.Id, Picture, null);
            }
            catch (Exception)
            {
                if (!await PictureExists(Picture.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> PictureExists(int id)
        {
            return (await _pictureService.GetPictureByIdAsync(id)).Success;
        }
    }
}
