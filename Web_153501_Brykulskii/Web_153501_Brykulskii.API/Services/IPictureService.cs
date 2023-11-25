using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;

namespace Web_153501_Brykulskii.API.Services;

public interface IPictureService
{
	public Task<ResponseData<ListModel<Picture>>> GetPictureListAsync(string? genreNormalizedName, int pageNo = 1, int pageSize = 3);
	public Task<ResponseData<Picture>> GetPictureByIdAsync(int id);
	public Task UpdatePictureAsync(int id, Picture picture);
	public Task DeletePictureAsync(int id);
	public Task<ResponseData<Picture>> CreatePictureAsync(Picture picture);
	public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile);
}
