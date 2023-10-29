using Microsoft.AspNetCore.Mvc.RazorPages;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Services.PictureService;

namespace Web_153501_Brykulskii.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IPictureService _pictureService;

        public IndexModel(IPictureService pictureService)
        {
            _pictureService = pictureService;
        }

        public IList<Picture> Pictures { get; set; } = default!;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public async Task OnGetAsync(int pageNo = 1)
        {
            var response = await _pictureService.GetPictureListAsync(null, pageNo);

            if (response.Success)
            {
                Pictures = response.Data!.Items!;
                CurrentPage = response.Data.CurrentPage;
                TotalPages = response.Data.TotalPages;
            }
        }
    }
}
