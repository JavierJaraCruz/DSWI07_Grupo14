using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Burgos0._2.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // GET: api/categoria
        [HttpGet]
        public async Task<ActionResult<List<Categoria>>> ListarCategorias()
        {
            var categorias =
                await _categoriaService.ListarCategorias();

            return Ok(categorias);
        }

        // GET: api/categoria/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Categoria>> ObtenerCategoria(int id)
        {
            var categoria =
                await _categoriaService.ObtenerCategoria(id);

            if (categoria == null)
                return NotFound();

            return Ok(categoria);
        }
    }
}