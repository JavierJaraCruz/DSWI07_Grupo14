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
        private readonly CompraDAL compraDAL = new CompraDAL();

        public int CrearCompra(int proveedorId, List<CompraDetalle> detalles)
            => compraDAL.InsertarCompra(proveedorId, detalles);

        public List<CompraProveedor> ListarCompras() => compraDAL.ListarCompras();

    }
}
