using Entities;

namespace Burgos0._2.Models
{
    public class DashboardViewModel
    {
        public int CantidadProductos { get; set; }
        public int CantidadVentas { get; set; }
        public int StockBajo { get; set; }
        public decimal TotalVentasMonto { get; set; }
        public string? MensajeEstado { get; set; }

        public Dashboard? Datos { get; set; }
    }
}