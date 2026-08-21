using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Burgos0._2.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductosController(ProductoService productoService)
        {
            _productoService = productoService;
        }

        // GET: api/productos
        [HttpGet]
        public async Task<ActionResult<List<Producto>>> ListarProductos()
        {
            var productos = await _productoService.ListarProductosAsync();

            return Ok(productos);
        }

        // GET: api/productos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Producto>> ObtenerProducto(int id)
        {
            var producto = await _productoService.ObtenerProductoAsync(id);

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }
    }
}