using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Lab04.WebsiteBanHang.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Lab04.WebsiteBanHang.Controllers
{
    [Authorize(Roles = SD.Role_Admin)]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: User/Index
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var userRoles = new List<UserRoleViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles.Add(new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    SelectedRole = roles.FirstOrDefault() // Chỉ lấy role đầu tiên
                });
            }

            return View(userRoles);
        }

        // GET: User/Edit/{id}
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault(); // Lấy 1 role duy nhất

            // Không cho phép chỉnh sửa nếu user đã là Admin
            if (currentRole == SD.Role_Admin)
            {
                TempData["ErrorMessage"] = "Không thể chỉnh sửa tài khoản Admin!";
                return RedirectToAction(nameof(Index));
            }

            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            var model = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                Address = user.Address,
                Age = user.Age,
                SelectedRole = currentRole,
                AvailableRoles = allRoles
            };

            return View(model);
        }

        // POST: User/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null) return NotFound();

                var currentRoles = await _userManager.GetRolesAsync(user);
                var currentRole = currentRoles.FirstOrDefault();

                // Nếu user đang là Admin, không cho phép thay đổi
                if (currentRole == SD.Role_Admin)
                {
                    TempData["ErrorMessage"] = "Không thể chỉnh sửa tài khoản Admin!";
                    return RedirectToAction(nameof(Index));
                }

                // Cập nhật thông tin người dùng
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Address = model.Address;
                user.Age = model.Age;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                // Cập nhật vai trò (chỉ được chọn 1 vai trò)
                if (!string.IsNullOrEmpty(model.SelectedRole))
                {
                    await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, model.SelectedRole);
                }

                TempData["SuccessMessage"] = "Cập nhật thông tin và vai trò thành công!";
                return RedirectToAction(nameof(Index));
            }

            // Nếu ModelState không hợp lệ, load lại danh sách vai trò
            model.AvailableRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();
            return View(model);
        }

        // GET: User/Delete/{id}
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault();

            // Không thể xóa Admin
            if (currentRole == SD.Role_Admin)
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản Admin!";
                return RedirectToAction(nameof(Index));
            }

            var model = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                Email = user.Email,
                SelectedRole = currentRole
            };

            return View(model);
        }

        // POST: User/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault();

            if (currentRole == SD.Role_Admin)
            {
                TempData["ErrorMessage"] = "Không thể xóa tài khoản Admin!";
                return RedirectToAction(nameof(Index));
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Xóa người dùng thành công!";
            }
            else
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra khi xóa người dùng.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FullName = model.FullName,
                    UserName = model.Email,
                    Email = model.Email,
                    Address = model.Address,
                    Age = model.Age
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_Customer); // Gán role mặc định
                    TempData["SuccessMessage"] = "Thêm người dùng thành công!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(model);
        }
    }
}
