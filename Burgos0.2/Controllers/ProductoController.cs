using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Burgos0._2.Models;

namespace Burgos0._2.Controllers
{
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


        public IActionResult Index()
        {
            var productos = _productoService.ListarProductos();

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


        public IActionResult Detalle(int id)
        {
            var producto = _productoService.ObtenerProducto(id);

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



        [HttpGet]
        public IActionResult Crear()
        {
            var vm = new ProductoViewModel();

            vm.Categorias = _categoriaService.ListarCategorias()
                .Select(c => new SelectListItem
                {
                    Value = c.CategoriaId.ToString(),
                    Text = c.Nombre
                });

            return View(vm);
        }



        [HttpPost]
        public IActionResult Crear(ProductoViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Categorias = _categoriaService.ListarCategorias()
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


            _productoService.CrearProducto(producto);

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public IActionResult Editar(int id)
        {
            var producto = _productoService.ObtenerProducto(id);

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


            vm.Categorias = _categoriaService.ListarCategorias()
                .Select(c => new SelectListItem
                {
                    Value = c.CategoriaId.ToString(),
                    Text = c.Nombre
                });


            return View(vm);
        }



        [HttpPost]
        public IActionResult Editar(ProductoViewModel vm)
        {
            Console.WriteLine("ENTRÓ AL EDITAR POST");

            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }

            if (!ModelState.IsValid)
            {
                vm.Categorias = _categoriaService.ListarCategorias()
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


            _productoService.ActualizarProducto(producto);

            return RedirectToAction(nameof(Index));
        }



        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var producto = _productoService.ObtenerProducto(id);

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



        [HttpPost]
        public IActionResult EliminarConfirmado(int ProductoId)
        {
            _productoService.EliminarProducto(ProductoId);

            return RedirectToAction(nameof(Index));
        }
    }
}
