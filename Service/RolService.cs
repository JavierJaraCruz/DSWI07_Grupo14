using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RolService
    {
        private readonly RolDAL _rolDAL;

        public RolService(RolDAL rolDAL)
        {
            _rolDAL = rolDAL;
        }

        public int CrearRol(Rol r) => _rolDAL.Insertar(r);

        public List<Rol> ListarRoles() => _rolDAL.Listar();

        public void EliminarRol(int id) => _rolDAL.Eliminar(id);
    }
}
