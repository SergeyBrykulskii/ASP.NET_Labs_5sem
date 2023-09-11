using Microsoft.AspNetCore.Mvc;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;
using Web_153501_Brykulskii.Services.PictureGenreService;

namespace Web_153501_Brykulskii.Services.PictureService;

public class MemoryPictureService : IPictureService
{
    private List<Picture>? _pictures;
    private List<PictureGenre>? _pictureGenres;
    private readonly IConfiguration _config;

    public MemoryPictureService(
        [FromServices] IConfiguration config,
        IPictureGenreService pictureGenreService)
    {
        _config = config;
        _pictureGenres = pictureGenreService.GetPictureGenreListAsync().Result.Data;
        SetupData();
    }
    public Task<ResponseData<Picture>> CreatePictureAsync(Picture picture, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    public Task DeletePictureAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<Picture>> GetPictureByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseData<ListModel<Picture>>> GetPictureListAsync(string? genreNormalizedName, int pageNo = 1)
    {
        var itemsPerPage = _config.GetValue<int>("ItemsPerPage");
        var itemsTemp = _pictures!.
            Where(c => genreNormalizedName == null || c.Genre?.NormalizedName == genreNormalizedName);
        int totalPages = itemsTemp.Count() / itemsPerPage +
            (itemsTemp.Count() % itemsPerPage == 0 ? 0 : 1);
        var items = itemsTemp
            .Skip((pageNo - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToList();

        return Task.FromResult(new ResponseData<ListModel<Picture>>
        {
            Success = true,
            Data = new ListModel<Picture>
            {
                Items = items,
                CurrentPage = pageNo,
                TotalPages = totalPages
            },
            ErrorMessage = string.Empty
        });
    }

    public Task UpdatePictureAsync(int id, Picture picture, IFormFile? formFile)
    {
        throw new NotImplementedException();
    }

    private void SetupData()
    {
        _pictures = new List<Picture>
        {
            new Picture
            {
                Id = 1,
                Name = "Мона Лиза",
                Author = "Леонардо да Винчи",
                Description="Картина Леонардо да Винчи, одно из самых известных произведений живописи",
                Price = 2500000000,
                ImagePath = "Images/MonaLiza.jpg",
                Genre = _pictureGenres?.Find(c => c.NormalizedName.Equals("portrait")),
                GenreId = 1
            },
            new Picture
            {
                Id = 2,
                Name = "Девятый вал",
                Author = "Айвазовский Иван Константинович",
                Description = "Одна из самых знаменитых картин русского художника-мариниста Ивана Айвазовского, хранится в Русском музее в Санкт-Петербурге",
                Price = 700000,
                ImagePath = "Images/NinthWave.jpg",
                Genre = _pictureGenres?.Find(c => c.NormalizedName.Equals("marina")),
                GenreId = 2
            },
            new Picture
            {
                Id = 3,
                Name = "Корзина цветов",
                Author = "Микеланджело Меризи да Караваджо",
                Description = "Это одна из немногих картин художника, сюжет которой — исключительно натюрморт",
                Price = 50000,
                ImagePath = "Images/BasketOfFlowers.jpg",
                Genre = _pictureGenres?.Find(c => c.NormalizedName.Equals("still-life")),
                GenreId = 3
            },
            new Picture
            {
                Id = 4,
                Name = "Лунная ночь на Днепре",
                Author = "Куинджи Архип Иванович",
                Description = "Пейзаж русского художника Архипа Куинджи, написанный в 1880 году",
                Price = 20000,
                ImagePath = "Images/MoonNight.jpg",
                Genre = _pictureGenres?.Find(c => c.NormalizedName.Equals("landscape")),
                GenreId = 4
            },
            new Picture
            {
                Id = 5,
                Name = "Девочка с персиками",
                Author = " Валентин Александрович Серов",
                Description = "Картина русского живописца Валентина Серова, написана в 1887 году, хранится в Государственной Третьяковской галерее",
                Price = 200000,
                ImagePath = "Images/GirlWithPeaches.jpg",
                Genre = _pictureGenres?.Find(c => c.NormalizedName.Equals("portrait")),
                GenreId = 1
            },
            new Picture
            {
                Id = 6,
                Name = "Автопортрет с отрезанным ухом и трубкой",
                Author = "Винсента ван Гога",
                Description = "Картина нидерландского и французского художника Винсента ван Гога. Написана во время пребывания ван Гога в Арле в январе 1889 года",
                Price = 80000000,
                ImagePath = "Images/SelfPortraitWithBandagedEar.jpg",
                Genre = _pictureGenres?.Find(c => c.NormalizedName.Equals("portrait")),
                GenreId = 1
            },
            new Picture
            {
                Id = 7,
                Name = "Девушка с жемчужной серёжкой",
                Author = "Ян Вермеер",
                Description = "Одна из наиболее известных картин нидерландского художника Яна Вермеера. Её часто называют северной или голландской Моной Лизой",
                Price = 12000000,
                ImagePath = "Images/GirlWithPearlEarring.jpg",
                Genre = _pictureGenres?.Find(c => c.NormalizedName.Equals("portrait")),
                GenreId = 1
            },
        };
    }
}
