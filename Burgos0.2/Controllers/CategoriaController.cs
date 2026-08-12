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

        public IActionResult Index()
        {
            var categorias = _categoriaService.ListarCategorias();

            var lista = categorias.Select(c => new CategoriaViewModel
            {
                CategoriaId = c.CategoriaId,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion
            }).ToList();

            return View(lista);
        }

        public IActionResult Detalle(int id)
        {
            var categoria = _categoriaService.ObtenerCategoria(id);

            if (categoria == null)
                return NotFound();

            var vm = new CategoriaViewModel
            {
                CategoriaId = categoria.CategoriaId,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new CategoriaViewModel());
        }

        [HttpPost]
        public IActionResult Crear(CategoriaViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var categoria = new Categoria
            {
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion
            };

            _categoriaService.CrearCategoria(categoria);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            var categoria = _categoriaService.ObtenerCategoria(id);

            if (categoria == null)
                return NotFound();

            var vm = new CategoriaViewModel
            {
                CategoriaId = categoria.CategoriaId,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Editar(CategoriaViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var categoria = new Categoria
            {
                CategoriaId = vm.CategoriaId,
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion
            };

            _categoriaService.ActualizarCategoria(categoria);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var categoria = _categoriaService.ObtenerCategoria(id);

            if (categoria == null)
                return NotFound();

            var vm = new CategoriaViewModel
            {
                CategoriaId = categoria.CategoriaId,
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult EliminarConfirmado(int CategoriaId)
        {
            _categoriaService.EliminarCategoria(CategoriaId);

            return RedirectToAction("Index");
        }
    }
}