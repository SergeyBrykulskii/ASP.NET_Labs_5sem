using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;

namespace Web_153501_Brykulskii.API.Services;

public interface IPictureGenreService
{
    public Task<ResponseData<List<PictureGenre>>> GetPictureGenreListAsync();
}
