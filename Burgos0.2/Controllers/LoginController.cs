using Burgos0._2.Models;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace Burgos0._2.Controllers
{
    public class LoginController : Controller
    {
        private readonly UsuarioService _usuarioService;

        public LoginController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            var usuario = _usuarioService.ObtenerUsuarioPorNombre(model.NombreUsuario);

            if (usuario == null)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                return View("Index", model);
            }

            var hashIntento = PasswordHelper.GenerarPasswordHash(model.Password, usuario.Salt);

            if (usuario.PasswordHash != hashIntento)
            {
                ModelState.AddModelError("", "Usuario o contraseña incorrectos.");
                return View("Index", model);
            }

            if (!usuario.Estado)
            {
                ModelState.AddModelError("", "Tu cuenta se encuentra inactiva. Contacta al administrador.");
                return View("Index", model);
            }

            string nombreRol = _usuarioService.ObtenerNombreRolPorUsuario(usuario.UsuarioId);

            if (nombreRol != "Administrador" && nombreRol != "Admin")
            {
                ModelState.AddModelError("", "Acceso denegado: Solo los usuarios con rol Administrador pueden ingresar al sistema.");
                return View("Index", model);
            }

            HttpContext.Session.SetInt32("UsuarioId", usuario.UsuarioId);
            HttpContext.Session.SetString("NombreUsuario", usuario.NombreUsuario);
            HttpContext.Session.SetString("Rol", nombreRol);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }
    }
}