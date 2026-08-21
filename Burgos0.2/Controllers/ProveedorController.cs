using Burgos0._2.Models;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Burgos0._2.Controllers
{
    [ValidarSesion]
    public class ProveedorController : Controller
    {
        private readonly ProveedorService _proveedorService;

        public ProveedorController(ProveedorService proveedorService)
        {
            _proveedorService = proveedorService;
        }

        public async Task<IActionResult> Index()
        {
            var proveedores = await _proveedorService.ListarProveedoresAsync();

            var lista = proveedores.Select(p => new ProveedorViewModel
            {
                ProveedorId = p.ProveedorId,
                Nombre = p.Nombre,
                Email = p.Email,
                Telefono = p.Telefono,
                Direccion = p.Direccion
            }).ToList();

            return View(lista);
        }

        [HttpGet]
        public async Task<IActionResult> Detalle(int id)
        {
            var proveedor = await _proveedorService.ObtenerProveedorAsync(id);

            if (proveedor == null)
                return NotFound();

            var vm = new ProveedorViewModel
            {
                ProveedorId = proveedor.ProveedorId,
                Nombre = proveedor.Nombre,
                Email = proveedor.Email,
                Telefono = proveedor.Telefono,
                Direccion = proveedor.Direccion
            };

            return View(vm);
        }

        [HttpGet]
        public IActionResult Crear()
        {
            return View(new ProveedorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ProveedorViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var proveedor = new Proveedor
            {
                Nombre = vm.Nombre,
                Email = vm.Email,
                Telefono = vm.Telefono,
                Direccion = vm.Direccion
            };

            await _proveedorService.CrearProveedorAsync(proveedor);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var proveedor = await _proveedorService.ObtenerProveedorAsync(id);

            if (proveedor == null)
                return NotFound();

            var vm = new ProveedorViewModel
            {
                ProveedorId = proveedor.ProveedorId,
                Nombre = proveedor.Nombre,
                Email = proveedor.Email,
                Telefono = proveedor.Telefono,
                Direccion = proveedor.Direccion
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ProveedorViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var proveedor = new Proveedor
            {
                ProveedorId = vm.ProveedorId,
                Nombre = vm.Nombre,
                Email = vm.Email,
                Telefono = vm.Telefono,
                Direccion = vm.Direccion
            };

            await _proveedorService.ActualizarProveedorAsync(proveedor);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var proveedor = await _proveedorService.ObtenerProveedorAsync(id);

            if (proveedor == null)
                return NotFound();

            var vm = new ProveedorViewModel
            {
                ProveedorId = proveedor.ProveedorId,
                Nombre = proveedor.Nombre,
                Email = proveedor.Email,
                Telefono = proveedor.Telefono,
                Direccion = proveedor.Direccion
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarConfirmado(int proveedorId)
        {
            await _proveedorService.EliminarProveedorAsync(proveedorId);

            return RedirectToAction(nameof(Index));
        }
    }
}