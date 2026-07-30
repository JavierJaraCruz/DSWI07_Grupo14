using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Carrito
    {
        [Key]
        public int CarritoId { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; } = string.Empty;

    }
}
