using Microsoft.EntityFrameworkCore;
using Web_153501_Brykulskii.Domain.Entities;

namespace Web_153501_Brykulskii.API.Data;

public class AppDbContext : DbContext
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{
		Database.EnsureCreated();
	}

	public DbSet<Picture> Pictures { get; set; }
	public DbSet<PictureGenre> Genres { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<PictureGenre>().HasMany(g => g.Pictures).WithOne(p => p.Genre).HasForeignKey(p => p.GenreId);
	}
}
