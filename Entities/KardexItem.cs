using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class KardexItem
    {
        public int ProductoId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required, StringLength(20)]
        public string TipoMovimiento { get; set; } // Entrada / Salida

        [Required, Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [StringLength(100)]
        public string Referencia { get; set; }

        [Range(0, int.MaxValue)]
        public int Saldo { get; set; }

    }
}
