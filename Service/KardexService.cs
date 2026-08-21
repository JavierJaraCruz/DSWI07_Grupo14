using DAL;
using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class KardexService
    {
        private readonly InventarioDAL _inventarioDAL;

        public KardexService(InventarioDAL inventarioDAL)
        {
            _inventarioDAL = inventarioDAL;
        }

        public async Task<List<KardexItem>> ObtenerKardex(int productoId)
        {
            return await _inventarioDAL.ObtenerKardexPorProductoAsync(productoId);
        }

        public async Task InsertarKardex(KardexItem item)
            => await _inventarioDAL.InsertarKardexAsync(item);

        public async Task<List<KardexItem>> ObtenerKardexPorProducto(int productoId)
            => await _inventarioDAL.ObtenerKardexPorProductoAsync(productoId);
    }
}