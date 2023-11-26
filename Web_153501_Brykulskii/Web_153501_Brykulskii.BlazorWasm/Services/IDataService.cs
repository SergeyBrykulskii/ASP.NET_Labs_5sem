using Web_153501_Brykulskii.Domain.Entities;

namespace Web_153501_Brykulskii.BlazorWasm.Services;

public interface IDataService
{
    public List<PictureGenre>? Genres { get; set; }
    public List<Picture>? Pictures { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }

    public event Action? OnPicturesChange;

    public Task GetPicturesListAsync(string? genreNormalizedName, int pageNo = 1);
    public Task<Picture?> GetPictureByIdAsync(int id);
    public Task GetGenreListAsync();
}
