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

        public async Task<int> CrearOrdenAsync(
           int usuarioId,
           List<OrdenDetalle> detalles)
           => await _ordenDAL.InsertarOrdenAsync(usuarioId, detalles);

        public async Task<List<Orden>> ListarOrdenesAsync(
            int pagina,
            int tamano)
            => await _ordenDAL.ListarOrdenesAsync(pagina, tamano);

        public async Task<List<Orden>> ListarOrdenesDeAsync(
            int usuarioId)
            => await _ordenDAL.ListarOrdenesDeAsync(usuarioId);

        public async Task<int> ContarOrdenesAsync()
            => await _ordenDAL.ContarOrdenesAsync();

        public async Task<Orden> ObtenerPorIdAsync(int id)
            => await _ordenDAL.ObtenerPorIdAsync(id);

        public async Task ActualizarEstadoAsync(
                int ordenId,
                string estado)
                => await _ordenDAL.ActualizarEstadoAsync(ordenId, estado);

    }
}
