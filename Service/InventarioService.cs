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

        public async Task<int> RegistrarMovimientoAsync(InventarioMovimiento mov)
             => await _inventarioDAL.InsertarMovimientoAsync(mov);

        public async Task<List<InventarioMovimiento>> ListarMovimientosAsync(int productoId)
            => await _inventarioDAL.ListarMovimientosAsync(productoId);

    }
}
