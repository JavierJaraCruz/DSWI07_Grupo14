using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace Services
{
    public class InventarioService
    {
        private readonly InventarioDAL inventarioDAL = new InventarioDAL();

        public int RegistrarMovimiento(InventarioMovimiento mov) => inventarioDAL.InsertarMovimiento(mov);

        public List<InventarioMovimiento> ListarMovimientos(int productoId) => inventarioDAL.ListarMovimientos(productoId);

    }
}
