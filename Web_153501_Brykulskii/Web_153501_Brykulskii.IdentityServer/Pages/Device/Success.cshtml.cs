using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_153501_Brykulskii.IdentityServer.Pages.Device
{
	[SecurityHeaders]
	[Authorize]
	public class SuccessModel : PageModel
	{
		public void OnGet()
		{
		}
	}
}