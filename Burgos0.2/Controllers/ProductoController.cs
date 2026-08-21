using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Burgos0._2.Models;

namespace Burgos0._2.Controllers
{
    [ValidarSesion]
    public class ProductoController : Controller
    {
        private readonly ProductoService _productoService;
        private readonly CategoriaService _categoriaService;

        public ProductoController(
            ProductoService productoService,
            CategoriaService categoriaService)
        {
            _productoService = productoService;
            _categoriaService = categoriaService;
        }

        // GET: Producto
        public async Task<IActionResult> Index()
        {
            var productos =
                await _productoService.ListarTodosLosProductosAsync();

            var lista = productos.Select(p => new ProductoViewModel
            {
                ProductoId = p.ProductoId,
                Nombre = p.Nombre,
                Descripcion = p.Descripcion,
                Precio = p.Precio,
                Stock = p.Stock,
                CategoriaId = p.CategoriaId,
                CategoriaNombre = p.CategoriaNombre,
                ImagenUrl = p.ImagenUrl,
                Activo = p.Activo
            }).ToList();

            return View(lista);
        }

        // GET: Producto/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            // Para el administrador: permite ver productos activos e inactivos
            var producto =
                await _productoService.ObtenerProductoAdminAsync(id);

            if (producto == null)
                return NotFound();

            var vm = new ProductoViewModel
            {
                ProductoId = producto.ProductoId,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = producto.CategoriaNombre,
                ImagenUrl = producto.ImagenUrl,
                Activo = producto.Activo
            };

            return View(vm);
        }

        // GET: Producto/Crear
        [HttpGet]
        public async Task<IActionResult> Crear()
        {
            var vm = new ProductoViewModel();

            vm.Categorias =
                (await _categoriaService.ListarSoloActivos())
                .Select(c => new SelectListItem
                {
                    Value = c.CategoriaId.ToString(),
                    Text = c.Nombre
                });

            return View(vm);
        }

        // POST: Producto/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ProductoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categorias =
                    (await _categoriaService.ListarSoloActivos())
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoriaId.ToString(),
                        Text = c.Nombre
                    });

                return View(vm);
            }

            var producto = new Producto
            {
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion,
                Precio = vm.Precio,
                Stock = vm.Stock,
                CategoriaId = vm.CategoriaId,
                ImagenUrl = vm.ImagenUrl,
                Activo = true
            };

            await _productoService.CrearProductoAsync(producto);

            return RedirectToAction(nameof(Index));
        }

        // GET: Producto/Editar/5
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            // Para el administrador: permite editar productos activos e inactivos
            var producto =
                await _productoService.ObtenerProductoAdminAsync(id);

            if (producto == null)
                return NotFound();

            var vm = new ProductoViewModel
            {
                ProductoId = producto.ProductoId,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = producto.CategoriaNombre,
                ImagenUrl = producto.ImagenUrl,
                Activo = producto.Activo
            };

            // Para editar permitimos ver todas las categorías
            vm.Categorias =
                (await _categoriaService.ListarCategorias())
                .Select(c => new SelectListItem
                {
                    Value = c.CategoriaId.ToString(),
                    Text = c.Nombre
                });

            return View(vm);
        }

        // POST: Producto/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ProductoViewModel vm)
        {
            Console.WriteLine("ENTRO AL EDITAR POST");

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            if (!ModelState.IsValid)
            {
                vm.Categorias =
                    (await _categoriaService.ListarSoloActivos())
                    .Select(c => new SelectListItem
                    {
                        Value = c.CategoriaId.ToString(),
                        Text = c.Nombre
                    });

                return View(vm);
            }

            var producto = new Producto
            {
                ProductoId = vm.ProductoId,
                Nombre = vm.Nombre,
                Descripcion = vm.Descripcion,
                Precio = vm.Precio,
                Stock = vm.Stock,
                CategoriaId = vm.CategoriaId,
                ImagenUrl = vm.ImagenUrl,
                Activo = vm.Activo
            };

            await _productoService.ActualizarProductoAsync(producto);

            return RedirectToAction(nameof(Index));
        }

        // GET: Producto/Eliminar/5
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto =
                await _productoService.ObtenerProductoAdminAsync(id);

            if (producto == null)
                return NotFound();

            var vm = new ProductoViewModel
            {
                ProductoId = producto.ProductoId,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Stock = producto.Stock,
                CategoriaId = producto.CategoriaId,
                CategoriaNombre = producto.CategoriaNombre,
                ImagenUrl = producto.ImagenUrl,
                Activo = producto.Activo
            };

            return View(vm);
        }

        // POST: Producto/EliminarConfirmado
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int ProductoId)
        {
            await _productoService.EliminarProductoAsync(ProductoId);

            return RedirectToAction(nameof(Index));
        }
    }
}