using Entities;
using Microsoft.AspNetCore.Mvc;
using Services;
using Burgos0._2.Models.ApiModel;

namespace Burgos0._2.Controllers.API
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public AuthController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (request == null)
                return BadRequest();

            Usuario usuario =
                await _usuarioService.ObtenerUsuarioPorNombreAsync(
                    request.NombreUsuario);

            if (usuario == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                });
            }

            // Validar si el usuario está activo
            if (!usuario.Estado)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Tu cuenta se encuentra inactiva. Contacta al administrador."
                });
            }

            string hashIngresado =
                PasswordHelper.GenerarPasswordHash(
                    request.Password,
                    usuario.Salt
                );

            if (hashIngresado != usuario.PasswordHash)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Contraseña incorrecta"
                });
            }

            string nombreRol =
                await _usuarioService.ObtenerNombreRolPorUsuarioAsync(
                    usuario.UsuarioId);

            return Ok(new
            {
                Success = true,
                UsuarioId = usuario.UsuarioId,
                NombreUsuario = usuario.NombreUsuario,
                Rol = nombreRol
            });
        }

        // POST: api/auth/cambiar-password
        [HttpPost("cambiar-password")]
        public async Task<IActionResult> CambiarPassword(
            CambiarPasswordRequest request)
        {
            if (request == null)
                return BadRequest();

            Usuario usuario =
                await _usuarioService.ObtenerUsuarioPorNombreAsync(
                    request.NombreUsuario);

            if (usuario == null)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                });
            }

            string hashActual =
                PasswordHelper.GenerarPasswordHash(
                    request.PasswordActual,
                    usuario.Salt);

            if (hashActual != usuario.PasswordHash)
            {
                return Ok(new
                {
                    Success = false,
                    Message = "La contraseña actual es incorrecta"
                });
            }

            string nuevoSalt =
                PasswordHelper.GenerarSalt();

            string nuevoHash =
                PasswordHelper.GenerarPasswordHash(
                    request.PasswordNueva,
                    nuevoSalt);

            usuario.Salt = nuevoSalt;
            usuario.PasswordHash = nuevoHash;

            await _usuarioService.ActualizarUsuarioAsync(usuario);

            return Ok(new
            {
                Success = true,
                Message = "Contraseña actualizada correctamente"
            });
        }
    }
}