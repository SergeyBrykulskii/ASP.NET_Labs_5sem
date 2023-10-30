using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Security.Claims;
using Web_153501_Brykulskii.IdentityServer.Data;
using Web_153501_Brykulskii.IdentityServer.Models;

namespace Web_153501_Brykulskii.IdentityServer
{
	public class SeedData
	{
		public static void EnsureSeedData(WebApplication app)
		{
			using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
			{
				var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
				context.Database.Migrate();

				var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

				List<string> roles = new()
				{
					"Admin",
					"User"
				};

				foreach (var role in roles)
				{
					if (!roleManager.RoleExistsAsync(role).Result)
					{
						var result = roleManager.CreateAsync(new IdentityRole(role)).Result;
						if (!result.Succeeded)
						{
							throw new Exception(result.Errors.First().Description);
						}
					}
				}

				var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
				var alice = userMgr.FindByNameAsync("alice").Result;
				if (alice == null)
				{
					alice = new ApplicationUser
					{
						UserName = "alice",
						Email = "AliceSmith@email.com",
						EmailConfirmed = true,
					};
					var result = userMgr.CreateAsync(alice, "Pass123$").Result;
					if (!result.Succeeded)
					{
						throw new Exception(result.Errors.First().Description);
					}

					result = userMgr.AddClaimsAsync(alice, new Claim[]{
							new Claim(JwtClaimTypes.Name, "Alice Smith"),
							new Claim(JwtClaimTypes.GivenName, "Alice"),
							new Claim(JwtClaimTypes.FamilyName, "Smith"),
							new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
						}).Result;
					if (!result.Succeeded)
					{
						throw new Exception(result.Errors.First().Description);
					}
					Log.Debug("alice created");
				}
				else
				{
					Log.Debug("alice already exists");
				}

				var bob = userMgr.FindByNameAsync("bob").Result;
				if (bob == null)
				{
					bob = new ApplicationUser
					{
						UserName = "bob",
						Email = "BobSmith@email.com",
						EmailConfirmed = true
					};
					var result = userMgr.CreateAsync(bob, "Pass123$").Result;
					if (!result.Succeeded)
					{
						throw new Exception(result.Errors.First().Description);
					}

					result = userMgr.AddClaimsAsync(bob, new Claim[]{
							new Claim(JwtClaimTypes.Name, "Bob Smith"),
							new Claim(JwtClaimTypes.GivenName, "Bob"),
							new Claim(JwtClaimTypes.FamilyName, "Smith"),
							new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
							new Claim("location", "somewhere")
						}).Result;
					if (!result.Succeeded)
					{
						throw new Exception(result.Errors.First().Description);
					}
					Log.Debug("bob created");
				}
				else
				{
					Log.Debug("bob already exists");
				}

				var defaultUser = userMgr.FindByEmailAsync("user@gmail.com").Result;
				if (defaultUser == null)
				{
					defaultUser = new ApplicationUser
					{
						UserName = "defaultUser",
						Email = "user@gmail.com",
						EmailConfirmed = true
					};

					var result = userMgr.CreateAsync(defaultUser, "Pass123$").Result;

					if (!result.Succeeded)
					{
						throw new Exception(result.Errors.First().Description);
					}

					result = userMgr.AddClaimsAsync(defaultUser, new Claim[]
					{
							new Claim(JwtClaimTypes.Name, "Default User"),
							new Claim(JwtClaimTypes.GivenName, "Default"),
							new Claim(JwtClaimTypes.FamilyName, "User"),
							new Claim(JwtClaimTypes.WebSite, "http://default.com"),
							new Claim("location", "somewhere")
						}).Result;

					if (!result.Succeeded)
					{
						throw new Exception(result.Errors.First().Description);
					}

					Log.Debug("defaultUser created");
					userMgr.AddToRoleAsync(defaultUser, "User").Wait();
				}
				else
				{
					Log.Debug("defaultUser already exists");
				}

				var admin = userMgr.FindByEmailAsync("admin@gmail.com").Result;

				if (admin == null)
				{
					admin = new ApplicationUser
					{
						UserName = "admin",
						Email = "admin@gmail.com",
						EmailConfirmed = true
					};

					var result = userMgr.CreateAsync(admin, "Pass123$").Result;

					if (!result.Succeeded)
					{
						throw new Exception(result.Errors.First().Description);
					}

					result = userMgr.AddClaimsAsync(admin, new Claim[]
					{
							new Claim(JwtClaimTypes.Name, "Admin User"),
							new Claim(JwtClaimTypes.GivenName, "Admin"),
							new Claim(JwtClaimTypes.FamilyName, "User"),
							new Claim(JwtClaimTypes.WebSite, "http://admin.com"),
							new Claim("location", "somewhere"),
							new Claim(JwtClaimTypes.Role, "Admin")
						}).Result;

					if (!result.Succeeded)
					{
						throw new Exception(result.Errors.First().Description);
					}

					Log.Debug("admin created");
					userMgr.AddToRoleAsync(admin, "Admin").Wait();
				}
				else
				{
					Log.Debug("admin already exists");
				}
			}
		}
	}
}