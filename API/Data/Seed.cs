using System.Text.Json;
using API.Entitites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Helpers;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            Console.WriteLine("Seeding users...");

            if (await userManager.Users.AnyAsync())
            {
                Console.WriteLine("Users already exist in the database. Skipping seeding.");
                return;
            }

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

            if (users == null) return;

            var roles = new List<AppRole>()
            {
                new() { Name= Constants.Member},
                new() { Name = Constants.Admin},
                new() { Name = Constants.Moderator}
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName!.ToLower();
                await userManager.CreateAsync(user, "Pa$$w0rd");
                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin",
                KnownAs = Constants.Admin,
                Gender = "Male",
                City = "AdminCity",
                Country = "AdminLand"
            };

            var result = await userManager.CreateAsync(admin, "Pa$$w0rd");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, Constants.Admin);
                await userManager.AddToRoleAsync(admin, Constants.Moderator);
            }

            // Add this line to check if users are being added
            Console.WriteLine($"Seeding {users.Count} users...");

            // Log the result
            Console.WriteLine("Users have been seeded successfully.");
        }
    }
}