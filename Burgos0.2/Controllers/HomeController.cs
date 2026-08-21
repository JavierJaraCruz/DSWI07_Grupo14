using Burgos0._2.Models;
using Microsoft.AspNetCore.Mvc;
using Services;
using Web.Services;

namespace Burgos0._2.Controllers
{
    [ValidarSesion]
    public class HomeController : Controller
    {
        private readonly DashboardService _dashboardService;

        public HomeController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public async Task<IActionResult> Index()
        {
            var data = await _dashboardService.ObtenerDashboard();

            var vm = new DashboardViewModel
            {
                CantidadProductos = data.CantidadProductos,
                CantidadVentas = data.CantidadVentas,
                StockBajo = data.StockBajo,
                TotalVentasMonto = data.TotalVentasMonto,
                MensajeEstado = _dashboardService.ObtenerMensajeEstado(data)
            };

            return View(vm);
        }

        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}