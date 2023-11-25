using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Web_153501_Brykulskii.Controllers;

public class IdentityController : Controller
{
    private readonly ILogger<IdentityController> _logger;

    public IdentityController(ILogger<IdentityController> logger)
    {
        _logger = logger;
    }

    public async Task Login()
    {
        await HttpContext.ChallengeAsync("oidc", new AuthenticationProperties
        {
            RedirectUri = Url.Action("Index", "Home")
        });
    }

    public async Task Register()
    {
        await HttpContext.ChallengeAsync("oidc", new AuthenticationProperties
        {
            RedirectUri = Url.Action("Index", "Home")
        });
    }

    [HttpPost]
    public async Task Logout()
    {
        await HttpContext.SignOutAsync("cookie");
        await HttpContext.SignOutAsync("oidc", new AuthenticationProperties
        {
            RedirectUri = Url.Action("Index", "Home")
        });
        _logger.LogDebug("User logged out");
    }
}
