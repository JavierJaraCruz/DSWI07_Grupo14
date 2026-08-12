using Burgos0._2.Models;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;

namespace Burgos0._2.Controllers
{
    [ValidarSesion]
    public class UsuarioController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public UsuarioController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: Usuario
        public IActionResult Index()
        {
            var usuarios = _usuarioService.ListarUsuarios();
            return View(usuarios);
        }

        // GET: Usuario/Detalle/5
        public IActionResult Detalle(int id)
        {
            var usuario = _usuarioService.ObtenerUsuario(id);

            if (usuario == null)
                return NotFound();

            ViewBag.RolNombre = _usuarioService.ObtenerNombreRolPorUsuario(id);

            return View(usuario);
        }

        // GET: Usuario/Crear
        [HttpGet]
        public IActionResult Crear()
        {
            ViewBag.Roles = new SelectList(
                _usuarioService.ListarRoles(),
                "RolId",
                "NombreRol"
            );

            return View();
        }

        // POST: Usuario/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(UsuarioEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var salt = PasswordHelper.GenerarSalt();

                var hash = PasswordHelper.GenerarPasswordHash(
                    "default123",
                    salt
                );

                var usuario = new Usuario
                {
                    NombreUsuario = model.NombreUsuario,
                    Email = model.Email,
                    Estado = model.Estado,
                    PasswordHash = hash,
                    Salt = salt,
                    FechaRegistro = DateTime.Now
                };

                int nuevoUsuarioId = _usuarioService.CrearUsuario(usuario);

                _usuarioService.AsignarRolAUsuario(
                    nuevoUsuarioId,
                    model.RolId
                );

                TempData["SuccessMessage"] =
                    "Usuario creado con éxito. La contraseña inicial es: default123";

                return RedirectToAction("Index");
            }

            ViewBag.Roles = new SelectList(
                _usuarioService.ListarRoles(),
                "RolId",
                "NombreRol"
            );

            return View(model);
        }

        // GET: Usuario/Editar/5
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuario = _usuarioService.ObtenerUsuario(id);

            if (usuario == null)
                return NotFound();

            var model = new UsuarioEditViewModel
            {
                UsuarioId = usuario.UsuarioId,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Estado = usuario.Estado
            };

            return View(model);
        }

        // POST: Usuario/Editar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(UsuarioEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = _usuarioService.ObtenerUsuario(model.UsuarioId);

                if (usuario == null)
                    return NotFound();

                usuario.NombreUsuario = model.NombreUsuario;
                usuario.Email = model.Email;
                usuario.Estado = model.Estado;

                _usuarioService.ActualizarUsuario(usuario);

                TempData["SuccessMessage"] =
                    "Usuario actualizado correctamente.";

                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Usuario/Eliminar/5
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            if (id <= 0)
                return BadRequest();

            var usuario = _usuarioService.ObtenerUsuario(id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        // POST: Usuario/Eliminar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarConfirmado(int id)
        {
            var usuario = _usuarioService.ObtenerUsuario(id);

            if (usuario == null)
                return NotFound();

            _usuarioService.EliminarUsuario(id);

            TempData["SuccessMessage"] =
                "Usuario eliminado correctamente.";

            return RedirectToAction("Index");
        }
    }
}