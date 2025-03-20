using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using THLTW_B2.Models;

namespace THLTW_B2.DataAccess
{
    public static class DataSeeder
    {
        public static async Task SeedData(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Danh sách vai trò
            string[] roles = { SD.Role_Admin, SD.Role_User };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Tạo Admin nếu chưa có
            await CreateUserIfNotExists(userManager, roleManager, 
                "admin@example.com", "Admin@123", "Admin", SD.Role_Admin);

            // Tạo User nếu chưa có
            await CreateUserIfNotExists(userManager, roleManager, 
                "user@example.com", "User@123", "User", SD.Role_User);
        }

        private static async Task CreateUserIfNotExists(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            string email, string password, string fullName, string role)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FullName = fullName,
                    Role = role
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                    Console.WriteLine($"✅ User {email} đã được tạo với vai trò {role}.");
                }
                else
                {
                    Console.WriteLine($"❌ Lỗi khi tạo user {email}: {string.Join(", ", result.Errors)}");
                }
            }
            else
            {
                Console.WriteLine($"⚠ User {email} đã tồn tại.");
            }
        }
    }
}
