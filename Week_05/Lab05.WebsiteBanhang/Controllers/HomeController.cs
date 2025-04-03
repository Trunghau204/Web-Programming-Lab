using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Lab04.WebsiteBanHang.Models;
using Lab04.WebsiteBanHang.Interfaces;

namespace Lab04.WebsiteBanHang.Controllers;

public class HomeController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public HomeController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productRepository.GetAllAsync();
        var categories = await _categoryRepository.GetAllAsync(); // Lấy danh sách Categories
        ViewBag.Categories = categories;
        return View(products);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View();
    }
}
