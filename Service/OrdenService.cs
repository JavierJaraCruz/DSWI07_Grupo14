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
        private readonly OrdenDAL _ordenDAL;

        public OrdenService(OrdenDAL ordenDAL)
        {
            _ordenDAL = ordenDAL;
        }

        public int CrearOrden(int usuarioId, List<OrdenDetalle> detalles)
            => _ordenDAL.InsertarOrden(usuarioId, detalles);

        public List<Orden> ListarOrdenes(int pagina, int tamano)
        {
            return _ordenDAL.ListarOrdenes(pagina, tamano);
        }
        public List<Orden> ListarOrdenesDe(int usuarioId)
        {
            return _ordenDAL.ListarOrdenesDe(usuarioId);
        }

        public int ContarOrdenes()
        {
            return _ordenDAL.ContarOrdenes();
        }

        public Orden ObtenerPorId(int id)
        {
            return _ordenDAL.ObtenerPorId(id);
        }

    }
}
