namespace Web_153501_Brykulskii.Domain.Entities;

public class PictureGenre
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string NormalizedName { get; set; } = string.Empty;
	public List<Picture> Pictures { get; set; } = new List<Picture>();
}
