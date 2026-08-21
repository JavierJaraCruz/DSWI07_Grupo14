using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ProveedorService
    {
        private readonly ProveedorDAL _proveedorDAL;

        public ProveedorService(ProveedorDAL proveedorDAL)
        {
            _proveedorDAL = proveedorDAL;
        }

        public async Task<int> CrearProveedorAsync(Proveedor p)
            => await _proveedorDAL.InsertarAsync(p);

        public async Task<List<Proveedor>> ListarProveedoresAsync()
            => await _proveedorDAL.ListarAsync();

        public async Task<Proveedor> ObtenerProveedorAsync(int id)
            => await _proveedorDAL.ObtenerProveedorAsync(id);

        public async Task ActualizarProveedorAsync(Proveedor p)
            => await _proveedorDAL.ActualizarAsync(p);

        public async Task EliminarProveedorAsync(int id)
            => await _proveedorDAL.EliminarAsync(id);
    }
}
