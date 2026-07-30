using DAL;
using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class OrdenService
    {
        private readonly OrdenDAL ordenDAL = new OrdenDAL();

        public int CrearOrden(int usuarioId, List<OrdenDetalle> detalles)
            => ordenDAL.InsertarOrden(usuarioId, detalles);

        public List<Orden> ListarOrdenes() => ordenDAL.ListarOrdenes();

        public Orden ObtenerPorId(int id)
        {
            return ordenDAL.ObtenerPorId(id);
        }

    }
}
