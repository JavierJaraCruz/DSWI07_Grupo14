using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UsuarioService
    {
        private readonly UsuarioDAL _usuarioDAL;

        public UsuarioService(UsuarioDAL usuarioDAL)
        {
            _usuarioDAL = usuarioDAL;
        }

        public async Task<int> CrearUsuarioAsync(Usuario u)
    => await _usuarioDAL.InsertarAsync(u);

        public async Task<Usuario> ObtenerUsuarioAsync(int id)
            => await _usuarioDAL.ObtenerPorIdAsync(id);

        public async Task<List<Usuario>> ListarUsuariosAsync()
            => await _usuarioDAL.ListarAsync();

        public async Task ActualizarUsuarioAsync(Usuario u)
            => await _usuarioDAL.ActualizarAsync(u);

        public async Task EliminarUsuarioAsync(int id)
            => await _usuarioDAL.EliminarAsync(id);

        public async Task<Usuario> ObtenerUsuarioPorNombreAsync(string nombreUsuario)
            => await _usuarioDAL.ObtenerPorNombreUsuarioAsync(nombreUsuario);

        public async Task AsignarRolAUsuarioAsync(int usuarioId, int rolId)
            => await _usuarioDAL.AsignarRolAsync(usuarioId, rolId);

        public async Task<string> ObtenerNombreRolPorUsuarioAsync(int usuarioId)
            => await _usuarioDAL.ObtenerNombreRolPorUsuarioAsync(usuarioId);

        public async Task<List<Rol>> ListarRolesAsync()
            => await _usuarioDAL.ListarRolesAsync();
    }
}
