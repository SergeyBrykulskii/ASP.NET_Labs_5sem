using Microsoft.EntityFrameworkCore;
using Web_153501_Brykulskii.Domain.Entities;

namespace Web_153501_Brykulskii.API.Data;

public class DbInitializer
{
	public static async Task SeedDataAsync(WebApplication app)
	{
		var genres = new List<PictureGenre>
		{
			new PictureGenre { Name="Портрет", NormalizedName="portrait" },
			new PictureGenre { Name="Пейзаж", NormalizedName="landscape" },
			new PictureGenre { Name="Марина", NormalizedName="marina" },
			new PictureGenre { Name="Натюрморт", NormalizedName="still-life" },

		};
		var portraitGenre = genres[0];
		var landscapeGenre = genres[1];
		var marinaGenre = genres[2];
		var stillLifeGenre = genres[3];

		var pictures = new List<Picture>
		{
			new Picture
			{
				Name = "Мона Лиза",
				Author = "Леонардо да Винчи",
				Description="Картина Леонардо да Винчи, одно из самых известных произведений живописи",
				Price = 2500000000,
				ImagePath = "MonaLiza.jpg",
				ImageMimeType = "image/jpeg",
				Genre = portraitGenre
			},
			new Picture
			{
				Name = "Девятый вал",
				Author = "Айвазовский Иван Константинович",
				Description = "Одна из самых знаменитых картин русского художника-мариниста Ивана Айвазовского, хранится в Русском музее в Санкт-Петербурге",
				Price = 700000,
				ImagePath = "NinthWave.jpg",
				ImageMimeType = "image/jpeg",
				Genre = marinaGenre
			},
			new Picture
			{
				Name = "Корзина цветов",
				Author = "Микеланджело Меризи да Караваджо",
				Description = "Это одна из немногих картин художника, сюжет которой — исключительно натюрморт",
				Price = 50000,
				ImagePath = "BasketOfFlowers.jpg",
				ImageMimeType = "image/jpeg",
				Genre = stillLifeGenre
			},
			new Picture
			{
				Name = "Лунная ночь на Днепре",
				Author = "Куинджи Архип Иванович",
				Description = "Пейзаж русского художника Архипа Куинджи, написанный в 1880 году",
				Price = 20000,
				ImagePath = "MoonNight.jpg",
				ImageMimeType = "image/jpeg",
				Genre = landscapeGenre
			},
			new Picture
			{
				Name = "Девочка с персиками",
				Author = " Валентин Александрович Серов",
				Description = "Картина русского живописца Валентина Серова, написана в 1887 году, хранится в Государственной Третьяковской галерее",
				Price = 200000,
				ImagePath = "GirlWithPeaches.jpg",
				ImageMimeType = "image/jpeg",
				Genre = portraitGenre
			},
			new Picture
			{
				Name = "Автопортрет с отрезанным ухом и трубкой",
				Author = "Винсента ван Гога",
				Description = "Картина нидерландского и французского художника Винсента ван Гога. Написана во время пребывания ван Гога в Арле в январе 1889 года",
				Price = 80000000,
				ImagePath = "SelfPortraitWithBandagedEar.jpg",
				ImageMimeType = "image/jpeg",
				Genre = portraitGenre
			},
			new Picture
			{
				Name = "Девушка с жемчужной серёжкой",
				Author = "Ян Вермеер",
				Description = "Одна из наиболее известных картин нидерландского художника Яна Вермеера. Её часто называют северной или голландской Моной Лизой",
				Price = 12000000,
				ImagePath = "GirlWithPearlEarring.jpg",
				ImageMimeType = "image/jpeg",
				Genre = portraitGenre
			},
		};
		using var scope = app.Services.CreateScope();
		var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

		if (context.Database.GetPendingMigrations().Any())
		{
			await context.Database.MigrateAsync();
		}

		if (!context.Genres.Any())
		{
			await context.Genres.AddRangeAsync(genres);
			await context.SaveChangesAsync();
		}

		if (!context.Pictures.Any())
		{
			var imagesUrl = app.Configuration.GetSection("ImagesUrl").Value;

			foreach (var picture in pictures)
			{
				picture.ImagePath = $"{imagesUrl}{picture.ImagePath}";
			}

			await context.Pictures.AddRangeAsync(pictures);
			await context.SaveChangesAsync();
		}
	}
}
