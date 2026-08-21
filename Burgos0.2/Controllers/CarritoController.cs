using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Burgos0._2.Controllers
{
    public class CarritoController : Controller
    {
        private readonly CarritoService _carritoService;

        public CarritoController(CarritoService carritoService)
        {
            _carritoService = carritoService;
        }

        public async Task<IActionResult> Crear(int usuarioId)
        {
            int id = await _carritoService.CrearCarrito(usuarioId);

            return RedirectToAction("Detalle", new { id });
        }

        public async Task<Carrito?> ObtenerPorUsuario(int usuarioId)
        {
            return await _carritoService.ObtenerPorUsuario(usuarioId);
        }

        public async Task<Carrito?> ObtenerCarrito(int id)
        {
            return await _carritoService.ObtenerCarrito(id);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var carrito = await _carritoService.ObtenerCarrito(id);

            return View(carrito);
        }

        public async Task<IActionResult> Eliminar(int id)
        {
            await _carritoService.EliminarCarrito(id);

            return RedirectToAction("Index", "Producto");
        }
    }
}