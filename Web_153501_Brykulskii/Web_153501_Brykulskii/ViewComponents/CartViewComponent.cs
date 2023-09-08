using Microsoft.AspNetCore.Mvc;

namespace Web_153501_Brykulskii.ViewComponents;

public class CartViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return await Task.FromResult<IViewComponentResult>(View());
    }
}
