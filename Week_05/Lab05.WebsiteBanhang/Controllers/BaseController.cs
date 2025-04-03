using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Lab04.WebsiteBanHang.Models;

namespace Lab04.WebsiteBanHang.Controllers
{
    [Authorize]
    public abstract class BaseController : Controller
    {
        protected bool IsAdmin => User.IsInRole(SD.Role_Admin);
        protected bool IsCustomer => User.IsInRole(SD.Role_Customer);
        protected bool IsEmployee => User.IsInRole(SD.Role_Employee);

        protected IActionResult AccessDenied()
        {
            return View("~/Views/Shared/AccessDenied.cshtml");
        }
    }
} 