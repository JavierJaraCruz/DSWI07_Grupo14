using Burgos0._2.Models;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Burgos0._2.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly CategoriaService _categoriaService;

        public CategoriaController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // LISTAR TODAS LAS CATEGORÍAS PARA EL ADMIN
        public async Task<IActionResult> Index()
        {
            var categorias =
                await _categoriaService.ListarCategorias();

            var lista = categorias.Select(c => new CategoriaViewModel
            {
                CategoriaId = c.CategoriaId,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Activo = c.Activo
            }).ToList();

            return View(lista);
        }

        // DETALLE
        public async Task<IActionResult> Detalle(int id)
        {
            var categoria =
                await _categoriaService.ObtenerCategoria(id);

            if (categoria == null)
                return NotFound();

            var vm = new CategoriaViewModel
            {
                CategoriaId = categoria.CategoriaId,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activo = categoria.Activo
            };

            return View(vm);
        }

        // GET: Crear
        [HttpGet]
        public IActionResult Crear()
        {
            return View(new CategoriaViewModel
            {
                Activo = true
            });
        }

        // POST: Crear
        [HttpPost]
        public async Task<IActionResult> Crear(CategoriaViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var categoria = new Categoria
            {
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion,
                Activo = true
            };

            await _categoriaService.CrearCategoria(categoria);

            return RedirectToAction("Index");
        }

        // GET: Editar
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var categoria =
                await _categoriaService.ObtenerCategoria(id);

            if (categoria == null)
                return NotFound();

            var vm = new CategoriaViewModel
            {
                CategoriaId = categoria.CategoriaId,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activo = categoria.Activo
            };

            return View(vm);
        }

        // POST: Editar
        [HttpPost]
        public async Task<IActionResult> Editar(CategoriaViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var categoria = new Categoria
            {
                CategoriaId = vm.CategoriaId,
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion,
                Activo = vm.Activo
            };

            await _categoriaService.ActualizarCategoria(categoria);

            return RedirectToAction("Index");
        }

        // GET: Eliminar
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var categoria =
                await _categoriaService.ObtenerCategoria(id);

            if (categoria == null)
                return NotFound();

            var vm = new CategoriaViewModel
            {
                CategoriaId = categoria.CategoriaId,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Activo = categoria.Activo
            };

            return View(vm);
        }

        // POST: Eliminación lógica
        [HttpPost]
        public async Task<IActionResult> EliminarConfirmado(int CategoriaId)
        {
            await _categoriaService.EliminarCategoria(CategoriaId);

            return RedirectToAction("Index");
        }
    }
}