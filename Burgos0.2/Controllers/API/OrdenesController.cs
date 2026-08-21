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
        public async Task<IActionResult> Comprar(int usuarioId)
        {
            try
            {
                var carrito =
                    await _carritoService.ObtenerPorUsuario(usuarioId);

                if (carrito == null)
                    return BadRequest("No existe carrito");

                var detallesCarrito =
                    await _carritoService.ObtenerDetalles(
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

                    await _productoService.ActualizarStockAsync(
                        item.ProductoId,
                        item.Cantidad,
                        "SALIDA",
                        "COMPRA"
                    );
                }

                int ordenId =
                    await _ordenService.CrearOrdenAsync(
                        usuarioId,
                        detallesOrden
                    );

                await _carritoService.VaciarCarrito(
                    carrito.CarritoId
                );

                await _carritoService.EliminarCarrito(
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
        public async Task<IActionResult> ListarPorUsuario(int usuarioId)
        {
            return Ok(
                await _ordenService.ListarOrdenesDeAsync(usuarioId)
            );
        }

        // GET: api/ordenes/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var orden =
                await _ordenService.ObtenerPorIdAsync(id);

            if (orden == null)
                return NotFound();

            return Ok(orden);
        }
    }
}