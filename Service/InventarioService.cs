using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.DAL;

namespace Services
{
    public class InventarioService
    {
        private readonly InventarioDAL _inventarioDAL;

        public InventarioService(InventarioDAL inventarioDAL)
        {
            _inventarioDAL = inventarioDAL;
        }

        public int RegistrarMovimiento(InventarioMovimiento mov) => _inventarioDAL.InsertarMovimiento(mov);

        public List<InventarioMovimiento> ListarMovimientos(int productoId) => _inventarioDAL.ListarMovimientos(productoId);

    }
}
