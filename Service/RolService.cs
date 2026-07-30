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
        private readonly RolDAL rolDAL = new RolDAL();

        public int CrearRol(Rol r) => rolDAL.Insertar(r);

        public List<Rol> ListarRoles() => rolDAL.Listar();

        public void EliminarRol(int id) => rolDAL.Eliminar(id);
    }
}
