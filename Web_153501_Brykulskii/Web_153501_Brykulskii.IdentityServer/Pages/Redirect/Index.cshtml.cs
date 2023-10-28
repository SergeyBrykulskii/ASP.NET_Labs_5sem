using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web_153501_Brykulskii.IdentityServer.Pages.Redirect
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public string RedirectUri { get; set; }

        public IActionResult OnGet(string redirectUri)
        {
            if (!Url.IsLocalUrl(redirectUri))
            {
                return RedirectToPage("/Home/Error/Index");
            }

            RedirectUri = redirectUri;
            return Page();
        }
    }
}