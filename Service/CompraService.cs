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

        public int CrearCompra(int proveedorId, List<CompraDetalle> detalles)
            => _compraDAL.InsertarCompra(proveedorId, detalles);

        public List<CompraProveedor> ListarCompras() => _compraDAL.ListarCompras();

    }
}
