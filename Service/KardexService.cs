using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public List<KardexItem> ObtenerKardex(int productoId)
        {
            return _inventarioDAL.ObtenerKardexPorProducto(productoId);
        }


        public void InsertarKardex(KardexItem item) => _inventarioDAL.InsertarKardex(item);

        public List<KardexItem> ObtenerKardexPorProducto(int productoId) => _inventarioDAL.ObtenerKardexPorProducto(productoId);
    }
}

