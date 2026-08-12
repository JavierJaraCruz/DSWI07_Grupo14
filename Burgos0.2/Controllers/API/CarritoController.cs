using Microsoft.AspNetCore.Mvc;
using Services;

namespace Burgos0._2.Controllers.API
{

    [ApiController]
    [Route("api/carrito")]
    public class CarritoController : ControllerBase
    {
        private readonly CarritoService _carritoService;
        private readonly ProductoService _productoService;

        public CarritoController(
            CarritoService carritoService,
            ProductoService productoService)
        {
            _carritoService = carritoService;
            _productoService = productoService;
        }

        // POST: api/carrito/agregar
        [HttpPost("agregar")]
        public IActionResult Agregar(CarritoRequest request)
        {
            if (request == null)
                return BadRequest("Request inválido");

            var carrito =
                _carritoService.ObtenerPorUsuario(
                    request.UsuarioId);

            int carritoId;

            if (carrito == null)
                carritoId =
                    _carritoService.CrearCarrito(
                        request.UsuarioId);
            else
                carritoId = carrito.CarritoId;

            var producto =
                _productoService.ObtenerProducto(
                    request.ProductoId);

            if (producto == null)
                return NotFound();

            _carritoService.AgregarProducto(
                carritoId,
                request.ProductoId,
                request.Cantidad,
                producto.Precio
            );

            return Ok(new
            {
                mensaje = "Agregado al carrito"
            });
        }

        // GET: api/carrito/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public IActionResult ObtenerCarrito(int usuarioId)
        {
            var carrito =
                _carritoService.ObtenerPorUsuario(usuarioId);

            if (carrito == null)
                return NotFound();

            var detalles =
                _carritoService.ObtenerDetalles(
                    carrito.CarritoId);

            return Ok(new
            {
                CarritoId = carrito.CarritoId,
                Productos = detalles
            });
        }
    }
}