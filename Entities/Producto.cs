using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Producto
    {
        [Key]
        public int ProductoId { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required, Range(0.01, 999999)]
        public decimal Precio { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        public string CategoriaNombre { get; set; }

        public bool Activo { get; set; }

        [StringLength(200)]
        public string ImagenUrl { get; set; }

    }
}
