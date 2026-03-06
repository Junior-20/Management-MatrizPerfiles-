using System.ComponentModel.DataAnnotations;

namespace MatrizPerfiles.Web.Models
{
    public class Sistema
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre del sistema es requerido.")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres.")]
        public string Nombre { get; set; } = string.Empty;
    }
}
