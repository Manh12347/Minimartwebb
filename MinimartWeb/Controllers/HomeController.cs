// Trong file: Controllers/HomeController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Thêm using này
using MinimartWeb.Data;          // Thêm using này
using MinimartWeb.Models;         // Thêm using này (hoặc .Model tùy cấu trúc)
using System.Diagnostics;
using System.Threading.Tasks;     // Thêm using này

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context; // Thêm DbContext

    // Sửa Constructor để nhận DbContext
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context; // Gán DbContext
    }

    // Sửa Action Index
    [AllowAnonymous] // Đảm bảo trang chủ công khai
    public async Task<IActionResult> Index() // Đổi thành async Task
    {
        // Lấy dữ liệu tương tự như ProductTypesController/Index
        var productTypes = await _context.ProductTypes
                                 .Include(p => p.Category)
                                 .Include(p => p.MeasurementUnit)
                                 .Include(p => p.Supplier)
                                 .AsNoTracking() // Thêm AsNoTracking nếu chỉ đọc
                                 .ToListAsync();

        // Truyền danh sách vào View
        return View(productTypes);
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}