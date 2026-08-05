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

        public int CrearUsuario(Usuario u) => _usuarioDAL.Insertar(u);

        public Usuario ObtenerUsuario(int id) => _usuarioDAL.ObtenerPorId(id);

        public List<Usuario> ListarUsuarios() => _usuarioDAL.Listar();

        public void ActualizarUsuario(Usuario u) => _usuarioDAL.Actualizar(u);

        public void EliminarUsuario(int id) => _usuarioDAL.Eliminar(id);

        public Usuario ObtenerUsuarioPorNombre(string nombreUsuario) => _usuarioDAL.ObtenerPorNombreUsuario(nombreUsuario);

        public void AsignarRolAUsuario(int usuarioId, int rolId) => _usuarioDAL.AsignarRol(usuarioId, rolId);

        public string ObtenerNombreRolPorUsuario(int usuarioId)
        {
     
            return _usuarioDAL.ObtenerNombreRolPorUsuario(usuarioId);
        }

        public List<Rol> ListarRoles() => _usuarioDAL.ListarRoles();
    }
}
