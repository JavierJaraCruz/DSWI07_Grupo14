using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Orden
    {
        [Key]
        public int OrdenId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaOrden { get; set; }

        [Required, Range(0.01, 999999)]
        public decimal Total { get; set; }

        [Required, StringLength(20)]
        public string Estado { get; set; }

    }
}
