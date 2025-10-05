using Microsoft.EntityFrameworkCore;
using TicketingService.EntityFramework.Models;
using static TicketingService.EntityFramework.Enumerations.Enums;

namespace TicketingService.EntityFramework
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext ctx)
        {
            if (await ctx.Users.AnyAsync()) return;

            var admin = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Admin",
                Email = "Admin@Email.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin"),
                Role = Role.Admin
            };

            var employee = new User
            {
                Id = Guid.NewGuid(),
                FullName = "Employee",
                Email = "Employee@Email.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Employee"),
                Role = Role.Employee
            };

            ctx.Users.AddRange(admin, employee);
            await ctx.SaveChangesAsync();
        }
    }

}
