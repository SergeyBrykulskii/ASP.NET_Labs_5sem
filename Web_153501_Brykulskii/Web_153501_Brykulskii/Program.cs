using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Web_153501_Brykulskii.Models;
using Web_153501_Brykulskii.Services.CartService;
using Web_153501_Brykulskii.Services.PictureGenreService;
using Web_153501_Brykulskii.Services.PictureService;

namespace Web_153501_Brykulskii;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddControllersWithViews();
		builder.Services.AddRazorPages();
		builder.Services.AddDistributedMemoryCache();
		builder.Services.AddSession();
		builder.Services.AddScoped(SessionCart.GetCart);
		builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

		UriData.ApiUri = builder.Configuration.GetSection("UriData")[key: "ApiUri"]!;

		builder.Services.AddHttpClient<IPictureService, ApiPictureService>(client =>
			client.BaseAddress = new Uri(UriData.ApiUri));

		builder.Services.AddHttpClient<IPictureGenreService, ApiPictureGenreService>(client =>
			client.BaseAddress = new Uri(UriData.ApiUri));

		builder.Services.AddHttpContextAccessor();

		builder.Services.AddAuthentication(opt =>
		{
			opt.DefaultScheme = "cookie";
			opt.DefaultChallengeScheme = "oidc";
		})
			.AddCookie("cookie")
			.AddOpenIdConnect("oidc", options =>
			{
				options.Authority =
				builder.Configuration["InteractiveServiceSettings:AuthorityUrl"];
				options.ClientId =
				builder.Configuration["InteractiveServiceSettings:ClientId"];
				options.ClientSecret =
				builder.Configuration["InteractiveServiceSettings:ClientSecret"];
				options.GetClaimsFromUserInfoEndpoint = true;
				options.ResponseType = "code";
				options.ResponseMode = "query";
				options.SaveTokens = true;

				options.ClaimActions.Add(new JsonKeyClaimAction("role", null, "role"));
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					NameClaimType = "name",
					RoleClaimType = "role"
				};
			});

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();

		app.UseRouting();

		app.UseSession();
		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllerRoute(
			name: "default",
			pattern: "{controller=Home}/{action=Index}/{id?}");
		app.MapRazorPages().RequireAuthorization();

		app.Run();
	}
}