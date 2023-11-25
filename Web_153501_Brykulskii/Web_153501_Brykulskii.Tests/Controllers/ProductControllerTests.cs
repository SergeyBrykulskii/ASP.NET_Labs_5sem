using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Web_153501_Brykulskii.Controllers;
using Web_153501_Brykulskii.Domain.Entities;
using Web_153501_Brykulskii.Domain.Models;
using Web_153501_Brykulskii.Services.PictureGenreService;
using Web_153501_Brykulskii.Services.PictureService;

namespace Web_153501_Brykulskii.Tests.Controllers;

class PictureGenreComparer : IEqualityComparer<PictureGenre>
{
	public bool Equals(PictureGenre? x, PictureGenre? y)
	{
		if (ReferenceEquals(x, y))
			return true;

		if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
			return false;

		return x.Id == y.Id && x.Name == y.Name && x.NormalizedName == y.NormalizedName;
	}

	public int GetHashCode(PictureGenre obj)
	{
		int hash = 17;
		hash = hash * 23 + obj.Id.GetHashCode();
		hash = hash * 23 + obj.Name.GetHashCode();
		hash = hash * 23 + obj.NormalizedName.GetHashCode();
		return hash;
	}
}
public class ProductControllerTests
{
	[Theory]
	[InlineData(true, false)]
	[InlineData(false, true)]
	public void Index_ServicesError_Returns404Error(bool genreServiseStatus, bool pictureServiseStatus)
	{
		// Arrange
		var mockPictureGenreService = Substitute.For<IPictureGenreService>();
		mockPictureGenreService.GetPictureGenreListAsync().Returns(new ResponseData<List<PictureGenre>>()
		{
			Success = genreServiseStatus
		});

		var mockPictureService = Substitute.For<IPictureService>();
		mockPictureService.GetPictureListAsync(null, 1).Returns(new ResponseData<ListModel<Picture>>()
		{
			Success = pictureServiseStatus
		});

		var controller = new ProductController(mockPictureGenreService, mockPictureService);

		// Act
		var result = controller.Index(null).Result;

		// Assert
		var viewResult = Assert.IsType<NotFoundObjectResult>(result);
		Assert.Equal(404, viewResult.StatusCode);
	}

	[Fact]
	public void Index_ServicesSuccessAnswer_ViewDataContainsGenres()
	{
		// Arrange
		var mockPictureGenreService = Substitute.For<IPictureGenreService>();
		mockPictureGenreService.GetPictureGenreListAsync().Returns(new ResponseData<List<PictureGenre>>()
		{
			Success = true,
			Data = GetSampleGenres()
		});

		var mockPictureService = Substitute.For<IPictureService>();
		mockPictureService.GetPictureListAsync(Arg.Any<string>(), 1).Returns(new ResponseData<ListModel<Picture>>()
		{
			Success = true,
			Data = new ListModel<Picture>()
			{
				Items = GetSamplePictures()
			}
		});

		var services = new ServiceCollection();
		services.AddMvc();
		services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
		services.AddSingleton<ILoggerFactory, LoggerFactory>();
		var serviceProvider = services.BuildServiceProvider();


		var controller = new ProductController(mockPictureGenreService, mockPictureService);
		controller.ControllerContext.HttpContext = new DefaultHttpContext
		{
			RequestServices = serviceProvider
		};

		// Act
		var result = controller.Index(null).Result;

		// Assert
		Assert.NotNull(result);
		var viewResult = Assert.IsType<ViewResult>(result);
		Assert.True(viewResult.ViewData.ContainsKey("genres"));
		Assert.Equal(GetSampleGenres(), viewResult.ViewData["genres"] as IEnumerable<PictureGenre>, new PictureGenreComparer());
	}

	[Theory]
	[InlineData(null, "Все")]
	[InlineData("portrait", "Портрет")]
	[InlineData("landscape", "Пейзаж")]
	[InlineData("still-life", "Натюрморт")]
	[InlineData("marina", "Марина")]
	public void Index_ServicesSuccessAnswer_ViewDataContainsCorrectCurrentGenre(string? genreRequest, string correctGenre)
	{
		// Arrange
		var mockPictureGenreService = Substitute.For<IPictureGenreService>();
		mockPictureGenreService.GetPictureGenreListAsync().Returns(new ResponseData<List<PictureGenre>>()
		{
			Success = true,
			Data = GetSampleGenres()
		});

		var mockPictureService = Substitute.For<IPictureService>();
		mockPictureService.GetPictureListAsync(Arg.Any<string>(), 1).Returns(new ResponseData<ListModel<Picture>>()
		{
			Success = true,
			Data = new ListModel<Picture>()
			{
				Items = GetSamplePictures()
			}
		});

		var services = new ServiceCollection();
		services.AddMvc();
		services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
		services.AddSingleton<ILoggerFactory, LoggerFactory>();
		var serviceProvider = services.BuildServiceProvider();


		var controller = new ProductController(mockPictureGenreService, mockPictureService);
		controller.ControllerContext.HttpContext = new DefaultHttpContext
		{
			RequestServices = serviceProvider
		};

		// Act
		var result = controller.Index(genreRequest).Result;

		// Assert
		Assert.NotNull(result);
		var viewResult = Assert.IsType<ViewResult>(result);
		Assert.True(viewResult.ViewData.ContainsKey("currentGenre"));
		Assert.Equal(correctGenre, viewResult.ViewData["currentGenre"]);

	}

	[Fact]
	public void Index_ServicesSuccessAnswer_ReturnsListOfPictures()
	{
		// Arrange
		var mockPictureGenreService = Substitute.For<IPictureGenreService>();
		mockPictureGenreService.GetPictureGenreListAsync().Returns(new ResponseData<List<PictureGenre>>()
		{
			Success = true,
			Data = GetSampleGenres()
		});

		var mockPictureService = Substitute.For<IPictureService>();
		mockPictureService.GetPictureListAsync(Arg.Any<string>(), 1).Returns(new ResponseData<ListModel<Picture>>()
		{
			Success = true,
			Data = new ListModel<Picture>()
			{
				Items = GetSamplePictures()
			}
		});

		var services = new ServiceCollection();
		services.AddMvc();
		services.AddSingleton<ITempDataDictionaryFactory, TempDataDictionaryFactory>();
		services.AddSingleton<ILoggerFactory, LoggerFactory>();
		var serviceProvider = services.BuildServiceProvider();


		var controller = new ProductController(mockPictureGenreService, mockPictureService);
		controller.ControllerContext.HttpContext = new DefaultHttpContext
		{
			RequestServices = serviceProvider
		};

		// Act
		var result = controller.Index(null).Result;

		// Assert
		Assert.NotNull(result);
		var viewResult = Assert.IsType<ViewResult>(result);
		Assert.IsType<List<Picture>>(viewResult.Model);
	}


	private List<PictureGenre> GetSampleGenres()
	{
		return new List<PictureGenre>() {
				new PictureGenre() { Id = 1, Name = "Портрет", NormalizedName = "potrait"},
				new PictureGenre() { Id = 2, Name = "Пейзаж", NormalizedName = "landscape"}
			};
	}

	private List<Picture> GetSamplePictures()
	{
		return new List<Picture>()
				{
					new Picture() { Id = 1, Price = 1000, Name = "Поле", GenreId = 2},
					new Picture() { Id = 1, Price = 10000, Name = "Мона", GenreId = 1},
				};
	}
}
