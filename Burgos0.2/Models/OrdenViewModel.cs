using Microsoft.AspNetCore.Mvc.Rendering;

namespace Burgos0._2.Models
{
    public class OrdenViewModel
    {
        public int OrdenId { get; set; }

        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }

        public decimal Total { get; set; }

        public DateTime FechaOrden { get; set; }

        public string Estado { get; set; }

        public List<OrdenDetalleViewModel> Detalles { get; set; }

        public IEnumerable<SelectListItem> Usuarios { get; set; }

        public IEnumerable<SelectListItem> Productos { get; set; }

        public OrdenViewModel()
        {
            Detalles = new List<OrdenDetalleViewModel>();
        }
    }
}
