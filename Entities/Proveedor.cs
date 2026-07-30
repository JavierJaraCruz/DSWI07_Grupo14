using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Proveedor
    {
        [Key]
        public int ProveedorId { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [EmailAddress, StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

    }
}
