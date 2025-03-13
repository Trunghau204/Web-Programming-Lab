using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using THLTW_B2.Models;
using THLTW_B2.Repositories;
using System.Threading.Tasks;

namespace THLTW_B2.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
      public async Task<IActionResult> Add(Product product, IFormFile ImageFile)
{
    var categories = await _categoryRepository.GetAllAsync();
    ViewBag.Categories = new SelectList(categories, "Id", "Name");

    if (ImageFile != null && ImageFile.Length > 0)
    {
        var fileName = Path.GetFileName(ImageFile.FileName);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await ImageFile.CopyToAsync(stream);
        }

        product.ImageUrl = "/images/" + fileName;
    }

    if (ModelState.IsValid)
    {
        await _productRepository.AddAsync(product);
        return RedirectToAction("Index");
    }

    return View(product);
}

        // Display a list of products 
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync();
            return View(products);
        }

        // Display a single product 
        public async Task<IActionResult> Display(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Show the product update form 
       [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound($"Không tìm thấy sản phẩm có ID: {id}");
            }

            // Lấy danh sách danh mục và đổ vào ViewBag
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId);

            return View(product);
        }


        // Process the product update 
        [HttpPost]
        public async Task<IActionResult> Update(Product product, IFormFile ImageFile)
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View(product); // Nếu dữ liệu không hợp lệ, trả lại form
            }

            var existingProduct = await _productRepository.GetByIdAsync(product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin sản phẩm
            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.CategoryId = product.CategoryId;

            // Kiểm tra nếu có ảnh mới được tải lên
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(ImageFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Cập nhật đường dẫn ảnh mới
                existingProduct.ImageUrl = "/images/" + fileName;
            }

            await _productRepository.UpdateAsync(existingProduct);
            return RedirectToAction("Index");
        }


        // Show the product delete confirmation 
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // Process the product deletion 
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _productRepository.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
