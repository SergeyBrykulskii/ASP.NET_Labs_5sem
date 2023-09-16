using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Services.PictureService;

namespace Web_153501_Brykulskii.Areas.Admin.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IPictureService _pictureService;

        public DeleteModel(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        [BindProperty]
        public Picture Picture { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var response = await _pictureService.GetPictureByIdAsync(id.Value);

            if (!response.Success)
            {
                return NotFound(response.ErrorMessage);
            }

            Picture = response.Data!;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // todo: если переделаю метод то добавить проверку
            await _pictureService.DeletePictureAsync(id.Value);

            return RedirectToPage("./Index");
        }
    }
}
