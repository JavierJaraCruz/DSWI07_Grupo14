using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Burgos0._2.Controllers
{
    public class CarritoController : Controller //EN REVISION, POSIBLEMENTE ESTE CONTROLLER SERA ELIMINADO,ADEMAS CONSIDERAR CAMBIAR ALGUNOS METODOS A PRIVATE POR TEMAS DE SEGURIDAD
    {
        private readonly CarritoService _carritoService;

        public CarritoController(CarritoService carritoService)
        { 
            _carritoService = carritoService;
        }


        public IActionResult Crear(int usuarioId)
        {
            int id = _carritoService.CrearCarrito(usuarioId);

            return RedirectToAction("Detalle", new { id });
        }


        public Carrito ObtenerPorUsuario(int usuarioId)
        {
            return _carritoService.ObtenerPorUsuario(usuarioId);
        }


        public Carrito ObtenerCarrito(int id)
        {
            return _carritoService.ObtenerCarrito(id);
        }


        public IActionResult Detalle(int id)
        {
            var carrito = _carritoService.ObtenerCarrito(id);

            return View(carrito);
        }


        public IActionResult Eliminar(int id)
        {
            _carritoService.EliminarCarrito(id);

            return RedirectToAction("Index", "Producto");
        }
    }
}
