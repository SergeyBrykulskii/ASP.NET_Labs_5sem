using Microsoft.AspNetCore.Mvc;

namespace Web_153501_Brykulskii.ViewComponents;

public class CartViewComponent : ViewComponent
{
	private readonly Domain.CartModels.Cart _sessionCart;

	public CartViewComponent(Domain.CartModels.Cart sessionCart)
	{
		_sessionCart = sessionCart;
	}
	public async Task<IViewComponentResult> InvokeAsync()
	{
		return await Task.FromResult<IViewComponentResult>(View(_sessionCart));
	}
}
