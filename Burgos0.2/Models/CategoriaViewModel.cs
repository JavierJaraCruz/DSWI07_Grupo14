using System.ComponentModel.DataAnnotations;

namespace Burgos0._2.Models
{
    public class CategoriaViewModel
    {
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        public bool Activo { get; set; }
    }
}