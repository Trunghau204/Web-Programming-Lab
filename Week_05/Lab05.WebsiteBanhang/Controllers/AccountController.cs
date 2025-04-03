    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Lab04.WebsiteBanHang.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc.Rendering;

    namespace Lab04.WebsiteBanHang.Controllers
    {
        public class AccountController : Controller
        {
            private readonly UserManager<ApplicationUser> _userManager;
            private readonly SignInManager<ApplicationUser> _signInManager;
            private readonly RoleManager<IdentityRole> _roleManager;

            public AccountController(
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                RoleManager<IdentityRole> roleManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
                _roleManager = roleManager;
            }

            [HttpGet]
            [AllowAnonymous]
            public async Task<IActionResult> Register()
            {
                var allowedRoles = new List<string> { SD.Role_Customer, SD.Role_Employee };
                var roles = await _roleManager.Roles
                    .Where(r => allowedRoles.Contains(r.Name))
                    .Select(r => new { r.Name })
                    .ToListAsync();
                ViewBag.Roles = new SelectList(roles, "Name", "Name");
                return View();
            }

            [HttpPost]
            [AllowAnonymous]
            public async Task<IActionResult> Register(RegisterViewModel model)
            {
                // Khai báo allowedRoles ở ngoài scope để sử dụng ở cả hai nơi
                var allowedRoles = new List<string> { SD.Role_Customer, SD.Role_Employee };

                if (ModelState.IsValid)
                {
                    if (model.Age.HasValue && (model.Age.Value < 17 || model.Age.Value > 100))
                    {
                        ModelState.AddModelError("Age", "Tuổi phải từ 17 đến 100");
                    }

                    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("Email", "Email đã được sử dụng.");
                    }

                    if (!allowedRoles.Contains(model.SelectedRole) || !await _roleManager.RoleExistsAsync(model.SelectedRole))
                    {
                        ModelState.AddModelError("SelectedRole", "Vai trò không hợp lệ hoặc không được phép.");
                    }

                    if (!ModelState.IsValid)
                    {
                        var roles = await _roleManager.Roles
                            .Where(r => allowedRoles.Contains(r.Name))
                            .Select(r => new { r.Name })
                            .ToListAsync();
                        ViewBag.Roles = new SelectList(roles, "Name", "Name", model.SelectedRole);
                        return View(model);
                    }

                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FullName = model.FullName,
                        Address = model.Address,
                        Age = model.Age
                    };

                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, model.SelectedRole);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToAction("Index", "Home");
                    }

                    foreach (var error in result.Errors)
                    {
                        if (error.Code.Contains("Password"))
                        {
                            ModelState.AddModelError("Password", error.Description);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                    var rolesOnError = await _roleManager.Roles
                        .Where(r => allowedRoles.Contains(r.Name))
                        .Select(r => new { r.Name })
                        .ToListAsync();
                    ViewBag.Roles = new SelectList(rolesOnError, "Name", "Name", model.SelectedRole);
                }

                // Gán ViewBag.Roles khi ModelState không hợp lệ ngay từ đầu
                var rolesOnInvalid = await _roleManager.Roles
                    .Where(r => allowedRoles.Contains(r.Name))
                    .Select(r => new { r.Name })
                    .ToListAsync();
                ViewBag.Roles = new SelectList(rolesOnInvalid, "Name", "Name", model.SelectedRole);
                return View(model);
            }

            [HttpGet]
            [AllowAnonymous]
            public IActionResult Login(string returnUrl = null)
            {
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }


            [HttpPost]
            [AllowAnonymous]

        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model); // Trả về model để hiển thị lại form
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["Message"] = "Email hoặc mật khẩu không chính xác.";
                TempData["AlertType"] = "danger"; // Màu đỏ
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                TempData["Message"] = "Đăng nhập thành công!";
                TempData["AlertType"] = "success"; // Màu xanh
                return RedirectToLocal(returnUrl);
            }
            else
            {
                TempData["Message"] = "Email hoặc mật khẩu không chính xác.";
                TempData["AlertType"] = "danger"; // Màu đỏ
                return View(model);
            }
        }


            
            [HttpPost]
            [Authorize]
            public async Task<IActionResult> Logout()
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            [HttpGet]
            [Authorize]
            public async Task<IActionResult> Manage()
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                var model = new ManageViewModel
                {
                    FullName = user.FullName,
                    Email = user.Email,
                    Address = user.Address,
                    Age = user.Age ?? 17
                };

                return View(model);
            }

            [HttpPost]
            [Authorize]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Manage(ManageViewModel model)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null) return NotFound();

                if (ModelState.IsValid)
                {
                    if (string.IsNullOrWhiteSpace(model.FullName))
                    {
                        ModelState.AddModelError("FullName", "Họ và tên không được để trống.");
                        model.Email = user.Email;
                        return View(model);
                    }

                    if (model.Age.HasValue && (model.Age.Value < 17 || model.Age.Value > 100))
                    {
                        ModelState.AddModelError("Age", "Tuổi phải từ 17 đến 100.");
                        model.Email = user.Email;
                        return View(model);
                    }

                    user.FullName = model.FullName;
                    user.Address = model.Address;
                    user.Age = model.Age;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
                        return RedirectToAction(nameof(Manage));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }

                model.Email = user.Email;
                return View(model);
            }

            private IActionResult RedirectToLocal(string returnUrl)
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }

           [HttpGet]
            [Authorize(Roles = SD.Role_Admin)]
            public async Task<IActionResult> Edit(string userId)
            {
                if (string.IsNullOrEmpty(userId))
                {
                    return NotFound();
                }

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }

                var userRoles = await _userManager.GetRolesAsync(user);
                bool isAdmin = userRoles.Contains(SD.Role_Admin);

                // Nếu user đã là Admin, không cho phép chỉnh sửa
                if (isAdmin)
                {
                    TempData["ErrorMessage"] = "Không thể chỉnh sửa tài khoản Quản trị viên!";
                    return RedirectToAction("Index");
                }

                var roles = new List<string> { SD.Role_Customer, SD.Role_Employee, SD.Role_Admin };

                var model = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    Email = user.Email,
                    Address = user.Address,
                    Age = user.Age,
                    SelectedRole = userRoles.FirstOrDefault(),
                    AvailableRoles = roles
                };

                return View(model);
            }

            [HttpPost]
            [Authorize(Roles = SD.Role_Admin)]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Edit(UserRoleViewModel model)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                // Nếu user đã là Admin, không cho phép chỉnh sửa
                if (userRoles.Contains(SD.Role_Admin))
                {
                    TempData["ErrorMessage"] = "Không thể chỉnh sửa tài khoản Quản trị viên!";
                    return RedirectToAction("Index");
                }

                // Chỉ cho phép chọn 1 role hợp lệ
                var allowedRoles = new List<string> { SD.Role_Customer, SD.Role_Employee, SD.Role_Admin };
                if (!allowedRoles.Contains(model.SelectedRole))
                {
                    TempData["ErrorMessage"] = "Vai trò không hợp lệ!";
                    return RedirectToAction("Index");
                }

                // Cập nhật thông tin người dùng
                user.FullName = model.FullName;
                user.Email = model.Email;
                user.Address = model.Address;
                user.Age = model.Age;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi cập nhật thông tin.");
                    return View(model);
                }

                // Xóa tất cả vai trò cũ
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, userRoles);
                if (!removeRolesResult.Succeeded)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi xóa vai trò cũ.");
                    return View(model);
                }

                // Thêm vai trò mới (chỉ 1)
                var addRoleResult = await _userManager.AddToRoleAsync(user, model.SelectedRole);
                if (!addRoleResult.Succeeded)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra khi thêm vai trò mới.");
                    return View(model);
                }

                TempData["SuccessMessage"] = "Cập nhật thành công!";
                return RedirectToAction("Index");
            }

            [HttpGet]
            [Authorize(Roles = SD.Role_Admin)]
            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            [Authorize(Roles = SD.Role_Admin)]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Create(RegisterViewModel model)
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                    return View(model);
                }

                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    Address = model.Address,
                    Age = model.Age
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, SD.Role_Customer); // Hoặc role khác
                    TempData["SuccessMessage"] = "Tạo tài khoản thành công!";
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

        }
    }