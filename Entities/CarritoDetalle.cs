using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CarritoDetalle
    {
        [Key]
        public int CarritoDetalleId { get; set; }

        [Required]
        public int CarritoId { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required, Range(0.01, 999999)]
        public decimal PrecioUnitario { get; set; }

        [Range(0.01, 999999)]
        public decimal Subtotal { get; set; }

    }
}
