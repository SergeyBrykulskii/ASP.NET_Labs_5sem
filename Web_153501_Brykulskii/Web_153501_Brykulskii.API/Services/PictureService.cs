using Microsoft.EntityFrameworkCore;
using Web_153501_Brykulskii.API.Data;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;

namespace Web_153501_Brykulskii.API.Services;

public class PictureService : IPictureService
{
    private readonly AppDbContext _context;
    private readonly int _maxPageSize = 20;

    public PictureService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ResponseData<ListModel<Picture>>> GetPictureListAsync(string? genreNormalizedName, int pageNo = 1, int pageSize = 3)
    {
        if (pageSize > _maxPageSize)
            pageSize = _maxPageSize;

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

    public Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
    {
        throw new NotImplementedException();
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
