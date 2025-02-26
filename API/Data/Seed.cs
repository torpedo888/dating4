using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entitites;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            Console.WriteLine("Seeding users...");

            if (await context.Users.AnyAsync())
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

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Password"));
                user.PasswordSalt = hmac.Key;

                context.Users.Add(user);
            }

            // Add this line to check if users are being added
            Console.WriteLine($"Seeding {users.Count} users...");

            await context.SaveChangesAsync();

            // Log the result
            Console.WriteLine("Users have been seeded successfully.");

        }
    }
}