using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Services
{
    public class CompraService
    {
        private readonly CompraDAL _compraDAL;

        public CompraService(CompraDAL compraDAL)
        {
            _compraDAL = compraDAL;
        }

        public async Task<int> CrearCompra(
            int proveedorId,
            List<CompraDetalle> detalles)
            => await _compraDAL.InsertarCompra(proveedorId, detalles);

        public async Task<List<CompraProveedor>> ListarCompras()
            => await _compraDAL.ListarCompras();

    }
}
