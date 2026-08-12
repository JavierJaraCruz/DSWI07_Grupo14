using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Burgos0._2.Controllers.API
{
    [ApiController]
    [Route("api/ordenes")]
    public class OrdenesController : ControllerBase
    {
        private readonly OrdenService _ordenService;
        private readonly CarritoService _carritoService;
        private readonly ProductoService _productoService;

        public OrdenesController(
            OrdenService ordenService,
            CarritoService carritoService,
            ProductoService productoService)
        {
            _ordenService = ordenService;
            _carritoService = carritoService;
            _productoService = productoService;
        }

        // POST: api/ordenes/comprar/5
        [HttpPost("comprar/{usuarioId}")]
        public IActionResult Comprar(int usuarioId)
        {
            try
            {
                var carrito =
                    _carritoService.ObtenerPorUsuario(usuarioId);

                if (carrito == null)
                    return BadRequest("No existe carrito");

                var detallesCarrito =
                    _carritoService.ObtenerDetalles(
                        carrito.CarritoId);

                if (detallesCarrito.Count == 0)
                    return BadRequest("Carrito vacío");

                List<OrdenDetalle> detallesOrden =
                    new List<OrdenDetalle>();

                foreach (var item in detallesCarrito)
                {
                    detallesOrden.Add(new OrdenDetalle
                    {
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = item.PrecioUnitario,
                        Subtotal = item.Subtotal
                    });

                    _productoService.ActualizarStock(
                        item.ProductoId,
                        item.Cantidad,
                        "SALIDA",
                        "COMPRA"
                    );
                }

                int ordenId =
                    _ordenService.CrearOrden(
                        usuarioId,
                        detallesOrden
                    );

                _carritoService.VaciarCarrito(
                    carrito.CarritoId
                );

                _carritoService.EliminarCarrito(
                    carrito.CarritoId
                );

                return Ok(new
                {
                    OrdenId = ordenId,
                    Mensaje = "Compra realizada"
                });
            }
            catch (Exception ex)
            {
                return Ok(new
                {
                    Error = ex.Message,
                    Inner = ex.InnerException?.Message
                });
            }
        }

        // GET: api/ordenes
        [HttpGet("usuario/{usuarioId}")]
        public IActionResult ListarPorUsuario(int usuarioId)
        {
            return Ok(
                _ordenService.ListarOrdenesDe(usuarioId)
            );
        }

        // GET: api/ordenes/5
        [HttpGet("{id:int}")]
        public IActionResult Obtener(int id)
        {
            var orden =
                _ordenService.ObtenerPorId(id);

            if (orden == null)
                return NotFound();

            return Ok(orden);
        }
    }
}
