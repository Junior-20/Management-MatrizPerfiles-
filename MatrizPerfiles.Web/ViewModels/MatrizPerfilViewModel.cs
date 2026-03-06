using MatrizPerfiles.Web.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MatrizPerfiles.Web.ViewModels
{
    public class MatrizPerfilViewModel
    {
        public PbiMatrizPerfil MatrizPerfil { get; set; } = new PbiMatrizPerfil();

        // Select lists for Dropdowns
        public IEnumerable<SelectListItem>? ListaSistemas { get; set; }
        public IEnumerable<SelectListItem>? ListaPuestos { get; set; }
    }
}
