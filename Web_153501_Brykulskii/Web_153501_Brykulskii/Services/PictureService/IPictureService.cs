using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;

namespace Web_153501_Brykulskii.Services.PictureService;

public interface IPictureService
{
	public Task<ResponseData<ListModel<Picture>>> GetPictureListAsync(string? genreNormalizedName, int pageNo = 1);
	public Task<ResponseData<Picture>> GetPictureByIdAsync(int id);
	public Task UpdatePictureAsync(int id, Picture picture, IFormFile? formFile);
	public Task DeletePictureAsync(int id);
	public Task<ResponseData<Picture>> CreatePictureAsync(Picture picture, IFormFile? formFile);
}
