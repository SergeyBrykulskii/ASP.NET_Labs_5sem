using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_153501_Brykulskii.Domain.CartModels;
using Web_153501_Brykulskii.Services.PictureService;

namespace Web_153501_Brykulskii.Controllers;

public class CartController : Controller
{
	private readonly IPictureService _pictureService;
	private readonly Cart _sessionCart;

	public CartController(
		IPictureService pictureService,
		Cart sessionCart)
	{
		_pictureService = pictureService;
		_sessionCart = sessionCart;
	}
	public IActionResult Index()
	{
		return View(_sessionCart);
	}

	[Route("[controller]/add/{id:int}")]
	[Authorize]
	public async Task<ActionResult> Add(int id, string returnUrl)
	{
		var data = await _pictureService.GetPictureByIdAsync(id);
		if (data.Success)
		{
			_sessionCart.Add(data.Data!);
		}
		return Redirect(returnUrl);
	}

	[Route("[controller]/remove/{id:int}")]
	[Authorize]
	public async Task<ActionResult> Remove(int id, string returnUrl)
	{
		var data = await _pictureService.GetPictureByIdAsync(id);
		if (data.Success)
		{
			_sessionCart.Remove(data.Data!.Id);
		}
		return Redirect(returnUrl);
	}
}
