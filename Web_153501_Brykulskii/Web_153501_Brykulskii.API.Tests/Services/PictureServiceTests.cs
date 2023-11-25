using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using Web_153501_Brykulskii.API.Data;
using Web_153501_Brykulskii.API.Services;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;

namespace Web_153501_Brykulskii.API.Tests.Services;

public class PictureServiceTests : IDisposable
{
	private readonly DbConnection _connection;
	private readonly DbContextOptions<AppDbContext> _contextOptions;

	public void Dispose() => _connection.Dispose();

	public PictureServiceTests()
	{
		_connection = new SqliteConnection("Filename=:memory:");
		_connection.Open();

		_contextOptions = new DbContextOptionsBuilder<AppDbContext>()
			.UseSqlite(_connection)
			.Options;

		using var context = new AppDbContext(_contextOptions);
		context.Database.EnsureCreated();
		context.Genres.AddRange(
			new PictureGenre() { Id = 1, Name = "Портрет", NormalizedName = "portrait" },
			new PictureGenre() { Id = 2, Name = "Пейзаж", NormalizedName = "landscape" });
		context.Pictures.AddRange(
			new Picture() { Id = 1, Price = 100, Name = "Поле", GenreId = 2 },
			new Picture() { Id = 2, Price = 100, Name = "Поле", GenreId = 2 },
			new Picture() { Id = 3, Price = 100, Name = "Поле", GenreId = 2 },
			new Picture() { Id = 4, Price = 1000, Name = "Саша", GenreId = 1 },
			new Picture() { Id = 5, Price = 1000, Name = "Саша", GenreId = 1 },
			new Picture() { Id = 6, Price = 10000, Name = "Таня", GenreId = 1 });
		context.SaveChanges();
	}

	private AppDbContext CreateContext() => new(_contextOptions);

	[Fact]
	public void GetPictureListAsync_ReturnsFirstPageWithThreeItems_WhenDefaultParametersPassed()
	{
		// Arrange
		using var context = CreateContext();
		PictureService service = new(context, null!, null!);

		// Act
		var result = service.GetPictureListAsync(null).Result;

		// Assert
		Assert.IsType<ResponseData<ListModel<Picture>>>(result);
		Assert.True(result.Success);
		Assert.Equal(1, result.Data!.CurrentPage);
		Assert.Equal(3, result.Data.Items.Count);
		Assert.Equal(2, result.Data.TotalPages);
		Assert.Equal(context.Pictures.First(), result.Data.Items[0]);
	}

	[Fact]
	public void GetPictureListAsync_ReturnsSecondPageWithSecondThreeItems_WhenPageParameterEquals2()
	{
		// Arrange
		using var context = CreateContext();
		PictureService service = new(context, null!, null!);

		// Act
		var result = service.GetPictureListAsync(null, 2).Result;

		// Assert
		Assert.IsType<ResponseData<ListModel<Picture>>>(result);
		Assert.True(result.Success);
		Assert.Equal(2, result.Data!.CurrentPage);
		Assert.Equal(3, result.Data.Items.Count);
		Assert.Equal(2, result.Data.TotalPages);
		Assert.Equal(context.Pictures.Skip(3).First(), result.Data.Items.First());
	}

	[Fact]
	public void GetPictureListAsync_ReturnsValidClothesByCategory_WhenCategoryParameterPassed()
	{
		// Arrange
		using var context = CreateContext();
		PictureService service = new(context, null!, null!);

		// Act
		var result = service.GetPictureListAsync("portrait").Result;

		// Assert
		Assert.IsType<ResponseData<ListModel<Picture>>>(result);
		Assert.True(result.Success);
		Assert.Equal(1, result.Data!.CurrentPage);
		Assert.Equal(3, result.Data.Items.Count);
		Assert.Equal(1, result.Data.TotalPages);
		Assert.DoesNotContain(result.Data.Items, x => x.GenreId != 1);
	}

	[Fact]
	public void GetPictureListAsync_ReturnsSuccessFalse_WhenPageNumberParameterIsGreaterThanTotalPages()
	{
		// Arrange
		using var context = CreateContext();
		PictureService service = new(context, null!, null!);

		// Act
		var result = service.GetPictureListAsync(null, 1000).Result;

		// Assert
		Assert.IsType<ResponseData<ListModel<Picture>>>(result);
		Assert.False(result.Success);
	}

	[Fact]
	public void GetPictureListAsync_DoesNotAllowToSetPageSizeGreaterThanMaxPageSize_WhenGreaterPageSizePassed()
	{
		// Arrange
		using var context = CreateContext();
		PictureService service = new(context, null!, null!);

		// Act
		var result = service.GetPictureListAsync(null!, 1, service.MaxPageSize + 1).Result;

		// Assert
		Assert.True(result.Success);
		Assert.True(result.Data!.Items.Count <= service.MaxPageSize);
	}

}
