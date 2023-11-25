using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Web_153501_Brykulskii.IdentityServer.Models;

namespace Web_153501_Brykulskii.IdentityServer.Controllers
{
	[Route("[controller]")]
	[ApiController]
	[Authorize]
	public class AvatarController : ControllerBase
	{
		private readonly IWebHostEnvironment _environment;
		private readonly UserManager<ApplicationUser> _userManager;

		public AvatarController(
			IWebHostEnvironment environment,
			UserManager<ApplicationUser> userManager)
		{
			_environment = environment;
			_userManager = userManager;
		}

		[HttpGet]
		public async Task<IActionResult> GetAvatar()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
				return NotFound();

			FileExtensionContentTypeProvider provider = new();

			var avatarPath = Path.Combine(_environment.WebRootPath, "Images", $"{user.Id}.png");
			provider.TryGetContentType(avatarPath, out string contentType);

			if (System.IO.File.Exists(avatarPath))
			{
				return PhysicalFile(avatarPath, contentType);
			}

			var defaultAvatarPath = Path.Combine(_environment.WebRootPath, "Images", "defaultAva.png");
			provider.TryGetContentType(defaultAvatarPath, out string defaultContentType);

			return PhysicalFile(defaultAvatarPath, defaultContentType);
		}
	}
}
