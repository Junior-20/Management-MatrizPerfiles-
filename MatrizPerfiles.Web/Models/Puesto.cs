using System.ComponentModel.DataAnnotations;

namespace MatrizPerfiles.Web.Models
{
    public class Puesto
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre del puesto es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
