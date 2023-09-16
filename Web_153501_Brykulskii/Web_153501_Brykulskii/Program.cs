using Web_153501_Brykulskii.Models;
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
        //builder.Services.AddScoped<IPictureGenreService, MemoryPictureGenreService>();
        //builder.Services.AddScoped<IPictureService, MemoryPictureService>();

        UriData.ApiUri = builder.Configuration.GetSection("UriData")[key: "ApiUri"]!;

        builder.Services.AddHttpClient<IPictureService, ApiPictureService>(client =>
            client.BaseAddress = new Uri(UriData.ApiUri));

        builder.Services.AddHttpClient<IPictureGenreService, ApiPictureGenreService>(client =>
            client.BaseAddress = new Uri(UriData.ApiUri));

        // to delete
        //var connectionString = "Data Source=app.db";
        //string dataDirectory = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
        //connectionString = string.Format(connectionString!, dataDirectory);
        //builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString).EnableSensitiveDataLogging());

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

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}