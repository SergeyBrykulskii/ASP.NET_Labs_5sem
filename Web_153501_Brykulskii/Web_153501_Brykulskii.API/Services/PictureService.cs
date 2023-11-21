using Microsoft.EntityFrameworkCore;
using Web_153501_Brykulskii.API.Data;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;

namespace Web_153501_Brykulskii.API.Services;

public class PictureService : IPictureService
{
	private readonly AppDbContext _context;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IWebHostEnvironment _webHostEnvironment;
	public int MaxPageSize { get; private set; } = 20;

	public PictureService(
		AppDbContext context,
		IHttpContextAccessor httpContextAccessor,
		IWebHostEnvironment webHostEnvironment
		)
	{
		_context = context;
		_httpContextAccessor = httpContextAccessor;
		_webHostEnvironment = webHostEnvironment;
	}

	public async Task<ResponseData<ListModel<Picture>>> GetPictureListAsync(string? genreNormalizedName, int pageNo = 1, int pageSize = 3)
	{
		if (pageSize > MaxPageSize)
			pageSize = MaxPageSize;

		var query = _context.Pictures.AsQueryable();
		var dataList = new ListModel<Picture>();

		query = query.Where(d => genreNormalizedName == null || d.Genre.NormalizedName.Equals(genreNormalizedName));

		var count = query.Count();

		if (count == 0)
		{
			return new ResponseData<ListModel<Picture>>
			{
				Data = dataList,
				Success = false,
				ErrorMessage = "No such pictures"
			};
		}

		int totalPages = (int)Math.Ceiling(count / (double)pageSize);

		if (pageNo > totalPages)
			return new ResponseData<ListModel<Picture>>
			{
				Data = null,
				Success = false,
				ErrorMessage = "No such page"
			};

		dataList.Items = await query
			.Skip((pageNo - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();
		dataList.CurrentPage = pageNo;
		dataList.TotalPages = totalPages;

		var response = new ResponseData<ListModel<Picture>>
		{
			Data = dataList,
			Success = true,
		};

		return response;
	}

	public async Task<ResponseData<Picture>> CreatePictureAsync(Picture picture)
	{
		await _context.Pictures.AddAsync(picture);
		await _context.SaveChangesAsync();

		return new ResponseData<Picture>()
		{
			Data = picture,
			Success = true
		};
	}

	public async Task DeletePictureAsync(int id)
	{
		var picture = await _context.Pictures.FindAsync(id);
		if (picture != null)
		{
			_context.Pictures.Remove(picture);
			await _context.SaveChangesAsync();
		}
		else
			throw new ArgumentException("Picture with such id not found");
	}

	public async Task<ResponseData<Picture>> GetPictureByIdAsync(int id)
	{
		var picture = await _context.Pictures.FindAsync(id);
		if (picture != null)
		{
			return new ResponseData<Picture>()
			{
				Data = picture,
				Success = true
			};
		}
		else
			return new ResponseData<Picture>()
			{
				Data = null,
				Success = false,
				ErrorMessage = "Picture with such id not found"
			};
	}

	public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
	{
		var responseData = new ResponseData<string>();
		var picture = await _context.Pictures.FindAsync(id);

		if (picture == null)
		{
			responseData.Success = false;
			responseData.ErrorMessage = "No item found";
			return responseData;
		}

		var host = "https://" + _httpContextAccessor.HttpContext?.Request.Host;
		var imageFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");

		if (formFile != null)
		{
			if (!string.IsNullOrEmpty(picture.ImagePath))
			{
				var prevImage = Path.GetFileName(picture.ImagePath);
				var prevImagePath = Path.Combine(imageFolder, prevImage);

				if (File.Exists(prevImagePath))
					File.Delete(prevImagePath);
			}



			var ext = Path.GetExtension(formFile.FileName);
			var fName = Path.ChangeExtension(Path.GetRandomFileName(), ext);
			var filePath = Path.Combine(imageFolder, fName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await formFile.CopyToAsync(stream);
			}

			picture.ImagePath = $"{host}/Images/{fName}";
			picture.ImageMimeType = formFile.ContentType;

			_context.Entry(picture).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		responseData.Data = picture.ImagePath;
		return responseData;
	}


	public async Task UpdatePictureAsync(int id, Picture picture)
	{

		var pictureToUpdate = await _context.Pictures.FindAsync(id);
		if (pictureToUpdate != null)
		{
			pictureToUpdate.Name = picture.Name;
			pictureToUpdate.Description = picture.Description;
			pictureToUpdate.Price = picture.Price;
			pictureToUpdate.Author = picture.Author;
			pictureToUpdate.Genre = picture.Genre;
			pictureToUpdate.GenreId = picture.GenreId;
			await _context.SaveChangesAsync();
		}
		else
			throw new ArgumentException("Picture with such id not found");
	}
}
