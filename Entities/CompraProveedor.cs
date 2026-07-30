using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class CompraProveedor
    {
        [Key]
        public int CompraId { get; set; }

        [Required]
        public int ProveedorId { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaCompra { get; set; }

        [Required, Range(0.01, 999999)]
        public decimal Total { get; set; }

        [Required, StringLength(20)]
        public string Estado { get; set; }

    }
}
