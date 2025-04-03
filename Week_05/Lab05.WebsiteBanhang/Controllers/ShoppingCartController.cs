using System;
using System.Linq;
using System.Threading.Tasks;
using Lab04.WebsiteBanHang.Interfaces;
using Lab04.WebsiteBanHang.Models;
using Lab04.WebsiteBanHang.Data;
using Lab04.WebsiteBanHang.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab04.WebsiteBanHang.Controllers
{
    [Authorize]
    [Route("cart")]
    public class ShoppingCartController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ShoppingCartController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager, IProductRepository productRepository)
        {
            _context = context;
            _userManager = userManager;
            _productRepository = productRepository;
        }

        // Hiển thị giỏ hàng và danh sách đơn hàng
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var orders = await _context.Orders
                    .Where(o => o.UserId == user.Id)
                    .ToListAsync();
                ViewBag.Orders = orders; // Truyền danh sách đơn hàng qua ViewBag
            }
            return View(cart);
        }

        // Hiển thị trang thanh toán
        [HttpGet("checkout")]
        public IActionResult Checkout()
        {
            return View(new Order());
        }

        // Xử lý thanh toán
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(Order order)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            if (cart == null || !cart.Items.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            order.UserId = user.Id;
            order.OrderDate = DateTime.UtcNow;
            order.TotalPrice = cart.Items.Sum(i => i.Price * i.Quantity);
            order.OrderDetails = cart.Items.Select(i => new OrderDetail
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                Price = i.Price
            }).ToList();

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");

            return View("OrderCompleted", order.Id);
        }

        // Thêm sản phẩm vào giỏ hàng (chỉ chấp nhận POST)
        [HttpPost("add")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            if (productId <= 0 || quantity <= 0)
            {
                TempData["Error"] = "Dữ liệu không hợp lệ!";
                return RedirectToAction("Index");
            }

            var product = await GetProductFromDatabase(productId);
            if (product == null)
            {
                TempData["Error"] = "Sản phẩm không tồn tại!";
                return RedirectToAction("Index");
            }

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.AddItem(new CartItem
                {
                    ProductId = productId,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            HttpContext.Session.SetObjectAsJson("Cart", cart);

            TempData["Success"] = "Sản phẩm đã được thêm vào giỏ hàng!";
            return RedirectToAction("Index");
        }

        // Lấy số lượng sản phẩm trong giỏ hàng
        [HttpGet("count")]
        public IActionResult GetCartCount()
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            int count = cart?.Items.Sum(i => i.Quantity) ?? 0;
            return Json(count);
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpPost("remove")]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();
            var item = cart.Items.FirstOrDefault(p => p.ProductId == productId);

            if (item != null)
            {
                cart.Items.Remove(item);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        // Hiển thị chi tiết đơn hàng
        [HttpGet("order-details/{id}")]
        public async Task<IActionResult> OrderDetails(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            if (order.UserId != user.Id)
            {
                return Forbid(); // Chỉ cho phép người dùng xem đơn hàng của chính họ
            }

            return View(order);
        }

        // Phương thức lấy sản phẩm từ database
        private async Task<Product> GetProductFromDatabase(int productId)
        {
            return await _productRepository.GetByIdAsync(productId);
        }
    }
}