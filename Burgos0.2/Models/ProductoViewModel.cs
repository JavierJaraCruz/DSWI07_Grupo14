using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Burgos0._2.Models
{
    public class ProductoViewModel //CONSIDERAR CAMBIAR EL NAMESPACE
    {
        public int ProductoId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        public string? CategoriaNombre { get; set; }//EN .NET FRAMEWORK NO ESTABA EL NULLABLE HABILITADO 

        public string ImagenUrl { get; set; } 

        public bool Activo { get; set; }

        public IEnumerable<SelectListItem>? Categorias { get; set; }
    }
}
