using System.Text.Json.Serialization;
using Web_153501_Brykulskii.Domain.CartModels;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Extensions;

namespace Web_153501_Brykulskii.Services.CartService;

public class SessionCart : Cart
{
	[JsonIgnore]
	public ISession? Session { get; set; }
	public override void Add(Picture picture)
	{
		base.Add(picture);
		Session?.Set("Cart", this);
	}
	public override void Remove(int id)
	{
		base.Remove(id);
		Session?.Set("Cart", this);
	}
	public override void Clear()
	{
		base.Clear();
		Session?.Remove("Cart");
	}
	public static SessionCart GetCart(IServiceProvider services)
	{
		var session = services.GetRequiredService<IHttpContextAccessor>().HttpContext?.Session;
		var cart = session?.Get<SessionCart>("Cart") ?? new SessionCart();
		cart.Session = session;
		return cart;
	}
}
