using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Direccion
    {
        [Key]
        public int DireccionId { get; set; }

        [Required]
        public int UsuariId { get; set; }

        [Required, StringLength(200)]
        public string Calle { get; set; }

        [Required, StringLength(100)]
        public string Ciudad { get; set; }

        [Required, StringLength(100)]
        public string Pais { get; set; }

        [Required, StringLength(20)]
        public string CodigoPostal { get; set; }


    }
}
