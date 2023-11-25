using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Web_153501_Brykulskii.IdentityServer.Models;

namespace Web_153501_Brykulskii.IdentityServer.Pages.Account.Register;

[AllowAnonymous]
public class Index : PageModel
{
	private readonly SignInManager<ApplicationUser> _signInManager;
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly ILogger<Index> _logger;
	private readonly IWebHostEnvironment _environment;

	public Index(
		UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager,
		ILogger<Index> logger,
		IWebHostEnvironment environment)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_logger = logger;
		_environment = environment;
	}

	[BindProperty]
	public InputModel Input { get; set; }

	public string ReturnUrl { get; set; }

	public IList<AuthenticationScheme> ExternalLogins { get; set; }

	public class InputModel
	{
		[Required]
		[EmailAddress]
		[Display(Name = "Email")]
		public string Email { get; set; }

		[Required]
		[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
		[DataType(DataType.Password)]
		[Display(Name = "Password")]
		public string Password { get; set; }

		[DataType(DataType.Password)]
		[Display(Name = "Confirm password")]
		[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
		public string ConfirmPassword { get; set; }

		public IFormFile? Image { get; set; }
	}

	public async Task OnGetAsync(string returnUrl = null)
	{
		ReturnUrl = returnUrl;
		ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
	}

	public async Task<IActionResult> OnPostAsync(string returnUrl = null)
	{
		returnUrl ??= Url.Content("~/");
		ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		if (ModelState.IsValid)
		{
			var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
			var result = await _userManager.CreateAsync(user, Input.Password);
			if (result.Succeeded)
			{
				_logger.LogInformation("User created a new account with password.");

				if (Input.Image != null)
				{
					await SaveImageAsync(user.Id);
				}
				else
				{
					_logger.LogInformation("-----> Image is null");
				}

				if (_userManager.Options.SignIn.RequireConfirmedAccount)
				{
					return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl });
				}
				else
				{
					await _signInManager.SignInAsync(user, isPersistent: false);
					return LocalRedirect(returnUrl);
				}
			}
			foreach (var error in result.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		// If we got this far, something failed, redisplay form
		return Page();
	}
	private async Task SaveImageAsync(string id)
	{
		var ext = Path.GetExtension(Input.Image.FileName);
		var fileName = Path.ChangeExtension(id, ext);
		var path = Path.Combine(_environment.WebRootPath, "Images", fileName);
		using var stream = System.IO.File.OpenWrite(path);
		await Input.Image.CopyToAsync(stream);
	}
}
