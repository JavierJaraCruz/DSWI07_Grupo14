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

        public async Task<int> CrearRolAsync(Rol r)
            => await _rolDAL.InsertarAsync(r);

        public async Task<List<Rol>> ListarRolesAsync()
            => await _rolDAL.ListarAsync();

        public async Task EliminarRolAsync(int id)
            => await _rolDAL.EliminarAsync(id);
    }
}
