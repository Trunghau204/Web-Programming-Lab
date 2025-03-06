using Microsoft.AspNetCore.Mvc;
using Lab02_WebsiteBanHang.Models;
using Lab02_WebsiteBanHang.Repositories;

namespace Lab02_WebsiteBanHang.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAllCategories();
            return View(categories);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Category category)
        {
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Category added successfully!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Display(int id)
        {
            var category = _categoryRepository.GetAllCategories().FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        public IActionResult Update(int id)
        {
            var category = _categoryRepository.GetAllCategories().FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public IActionResult Update(Category category)
        {
            if (ModelState.IsValid)
            {
                TempData["SuccessMessage"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var category = _categoryRepository.GetAllCategories().FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public IActionResult DeleteConfirmed(int id)
        {
            var category = _categoryRepository.GetAllCategories().FirstOrDefault(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Category deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}