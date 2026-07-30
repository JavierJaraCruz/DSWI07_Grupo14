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
        private readonly UsuarioDAL usuarioDAL = new UsuarioDAL();

        public int CrearUsuario(Usuario u) => usuarioDAL.Insertar(u);

        public Usuario ObtenerUsuario(int id) => usuarioDAL.ObtenerPorId(id);

        public List<Usuario> ListarUsuarios() => usuarioDAL.Listar();

        public void ActualizarUsuario(Usuario u) => usuarioDAL.Actualizar(u);

        public void EliminarUsuario(int id) => usuarioDAL.Eliminar(id);

        public Usuario ObtenerUsuarioPorNombre(string nombreUsuario) => usuarioDAL.ObtenerPorNombreUsuario(nombreUsuario);

        public void AsignarRolAUsuario(int usuarioId, int rolId) => usuarioDAL.AsignarRol(usuarioId, rolId);

        public string ObtenerNombreRolPorUsuario(int usuarioId)
        {
     
            return usuarioDAL.ObtenerNombreRolPorUsuario(usuarioId);
        }

        public List<Rol> ListarRoles() => usuarioDAL.ListarRoles();
    }
}
