using Burgos0._2.Models;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;

namespace Burgos0._2.Controllers
{
    [ValidarSesion]
    public class OrdenController : Controller
    {
        private readonly OrdenService _ordenService;
        private readonly UsuarioService _usuarioService;

        public OrdenController(
            OrdenService ordenService,
            UsuarioService usuarioService)
        {
            _ordenService = ordenService;
            _usuarioService = usuarioService;
        }

        // GET: Orden
        public async Task<IActionResult> Index(int pagina = 1)
        {
            int tamano = 10;

            var ordenes = await _ordenService.ListarOrdenesAsync(
                pagina,
                tamano
            );

            var lista = ordenes.Select(o => new OrdenViewModel
            {
                OrdenId = o.OrdenId,
                UsuarioId = o.UsuarioId,
                NombreUsuario = o.NombreUsuario,
                FechaOrden = o.FechaOrden,
                Total = o.Total,
                Estado = o.Estado
            }).ToList();

            int totalOrdenes = await _ordenService.ContarOrdenesAsync();

            int totalPaginas = (int)Math.Ceiling(
                (double)totalOrdenes / tamano
            );

            ViewBag.Pagina = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(lista);
        }

        // GET: Orden/Crear
        public async Task<IActionResult> Crear()
        {
            var model = new OrdenViewModel();

            var usuarios = await _usuarioService.ListarUsuariosAsync();

            model.Usuarios = usuarios
                .Select(u => new SelectListItem
                {
                    Value = u.UsuarioId.ToString(),
                    Text = u.NombreUsuario
                })
                .ToList();

            model.Detalles.Add(new OrdenDetalleViewModel());

            return View(model);
        }

        // POST: Orden/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(OrdenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<OrdenDetalle> detalles = model.Detalles
                .Select(d => new OrdenDetalle
                {
                    ProductoId = d.ProductoId,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario,
                    Subtotal = d.Subtotal
                })
                .ToList();

            await _ordenService.CrearOrdenAsync(
                model.UsuarioId,
                detalles
            );

            return RedirectToAction("Index");
        }

        // GET: Orden/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var orden = await _ordenService.ObtenerPorIdAsync(id);

            if (orden == null)
                return NotFound();

            var model = new OrdenViewModel
            {
                OrdenId = orden.OrdenId,
                UsuarioId = orden.UsuarioId,
                NombreUsuario = orden.NombreUsuario,
                FechaOrden = orden.FechaOrden,
                Total = orden.Total,
                Estado = orden.Estado
            };

            return View(model);
        }
    }
}