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
        public ActionResult<List<Producto>> ListarProductos()
        {
            var productos = _productoService.ListarProductos();
            return Ok(productos);
        }

        // GET: api/productos/5
        [HttpGet("{id}")]
        public ActionResult<Producto> ObtenerProducto(int id)
        {
            var producto = _productoService.ObtenerProducto(id);

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }
    }
}