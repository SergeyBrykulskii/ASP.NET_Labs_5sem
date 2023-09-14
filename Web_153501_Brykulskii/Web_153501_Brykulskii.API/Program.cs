using Microsoft.EntityFrameworkCore;
using Web_153501_Brykulskii.API.Data;
using Web_153501_Brykulskii.API.Services;

namespace Web_153501_Brykulskii.API;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var connStr = builder.Configuration.GetConnectionString("Default");
        var dataDirectory = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar;
        connStr = string.Format(connStr!, dataDirectory);

        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connStr));
        builder.Services.AddControllers();
        builder.Services.AddScoped<IPictureService, PictureService>();
        builder.Services.AddScoped<IPictureGenreService, PictureGenreService>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseAuthorization();

        app.MapControllers();

        await DbInitializer.SeedDataAsync(app);

        app.Run();
    }
}