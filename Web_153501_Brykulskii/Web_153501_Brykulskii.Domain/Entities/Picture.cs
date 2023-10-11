namespace Web_153501_Brykulskii.Domain.Entities;

public class Picture
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Author { get; set; }
    public string? Description { get; set; }
    public uint? Price { get; set; }
    public int GenreId { get; set; }
    public PictureGenre Genre { get; set; }
    public string? ImagePath { get; set; }
    public string? ImageMimeType { get; set; }
}
