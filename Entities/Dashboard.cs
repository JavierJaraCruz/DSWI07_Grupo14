using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Dashboard
    {
        public int CantidadProductos { get; set; }
        public int CantidadVentas { get; set; }
        public int StockBajo { get; set; }
        public decimal TotalVentasMonto { get; set; }
    }
}
