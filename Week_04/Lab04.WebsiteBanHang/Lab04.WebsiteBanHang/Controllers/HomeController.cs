using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using THLTW_B2.Models;
using THLTW_B2.Repositories;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace THLTW_B2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductRepository _productRepository;

        public HomeController(ILogger<HomeController> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        // Đánh dấu là async và gọi đúng phương thức GetAllAsync()
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllAsync(); // Đúng với IProductRepository
            return View(products);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        

    }
}
