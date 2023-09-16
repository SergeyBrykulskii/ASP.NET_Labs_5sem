using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Services.PictureService;

namespace Web_153501_Brykulskii.Areas.Admin.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly IPictureService _pictureService;

        public DetailsModel(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        public Picture Picture { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _pictureService.GetPictureByIdAsync(id.Value);

            if (!response.Success)
            {
                return NotFound(response.ErrorMessage);
            }

            Picture = response.Data!;

            return Page();
        }
    }
}
