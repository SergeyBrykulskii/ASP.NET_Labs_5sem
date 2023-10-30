using Web_153501_Brykulskii.Domain.Entities;

namespace Web_153501_Brykulskii.Domain.CartModels;

public class Cart
{
	public Dictionary<int, CartItem> CartItems { get; set; } = new();

	public virtual void Add(Picture picture)
	{
		if (CartItems.ContainsKey(picture.Id))
		{
			CartItems[picture.Id].Quantity++;
		}
		else
		{
			CartItems.Add(picture.Id, new CartItem { Picture = picture, Quantity = 1 });
		}
	}

	public virtual void Remove(int id)
	{
		if (--CartItems[id].Quantity <= 0)
		{
			CartItems.Remove(id);
		}
	}

	public virtual void Clear()
	{
		CartItems.Clear();
	}

	public int Quontity => CartItems.Sum(item => item.Value.Quantity);

	public decimal TotalPrice => (decimal)CartItems.Sum(item => item.Value.Picture.Price * item.Value.Quantity)!;
}
