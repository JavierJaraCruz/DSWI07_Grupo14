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
        private readonly InventarioDAL inventarioDAL = new InventarioDAL();

        public List<KardexItem> ObtenerKardex(int productoId)
        {
            return inventarioDAL.ObtenerKardexPorProducto(productoId);
        }


        public void InsertarKardex(KardexItem item) => inventarioDAL.InsertarKardex(item);

        public List<KardexItem> ObtenerKardexPorProducto(int productoId) => inventarioDAL.ObtenerKardexPorProducto(productoId);
    }
}

