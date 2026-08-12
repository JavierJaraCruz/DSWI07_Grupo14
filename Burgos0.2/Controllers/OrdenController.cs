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
        public IActionResult Index(int pagina = 1)
        {
            int tamano = 10;

            var ordenes = _ordenService.ListarOrdenes(pagina, tamano);

            var lista = ordenes.Select(o => new OrdenViewModel
            {
                OrdenId = o.OrdenId,
                UsuarioId = o.UsuarioId,
                NombreUsuario = o.NombreUsuario,
                FechaOrden = o.FechaOrden,
                Total = o.Total,
                Estado = o.Estado
            }).ToList();

            int totalOrdenes = _ordenService.ContarOrdenes();

            int totalPaginas = (int)Math.Ceiling(
                (double)totalOrdenes / tamano
            );

            ViewBag.Pagina = pagina;
            ViewBag.TotalPaginas = totalPaginas;

            return View(lista);
        }

        // GET: Orden/Crear
        public IActionResult Crear()
        {
            var model = new OrdenViewModel();

            model.Usuarios = _usuarioService.ListarUsuarios()
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
        public IActionResult Crear(OrdenViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            List<OrdenDetalle> detalles = model.Detalles.Select(d => new OrdenDetalle
            {
                ProductoId = d.ProductoId,
                Cantidad = d.Cantidad,
                PrecioUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal
            }).ToList();

            _ordenService.CrearOrden(model.UsuarioId, detalles);

            return RedirectToAction("Index");
        }

        // GET: Orden/Detalle/5
        public IActionResult Detalle(int id)
        {
            var orden = _ordenService.ObtenerPorId(id);

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