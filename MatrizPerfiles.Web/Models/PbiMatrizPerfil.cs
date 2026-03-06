using System.ComponentModel.DataAnnotations;

namespace MatrizPerfiles.Web.Models
{
    public class PbiMatrizPerfil
    {
        public int Id { get; set; }

        public string? Empresa { get; set; }
        public string? DescripcionUnidadOrganizativa { get; set; }
        public string? Codigo { get; set; }
        
        public int? SistemaId { get; set; }
        public Sistema? Sistema { get; set; }
        
        public int? PuestoId { get; set; }
        public Puesto? Puesto { get; set; }
        
        public string? Modulo { get; set; }
        public string? SubModulo { get; set; }
        public string? Menu { get; set; }
        public string? Sitio { get; set; }
        public string? SubSitio { get; set; }
        public string? OpcionDeMenu { get; set; }
        public string? Opcion { get; set; }
        public string? Programa { get; set; }
        public string? ArchivoDePrograma { get; set; }
        public string? FuncionRolAsociado { get; set; }
        public string? CodigoIdFuncion { get; set; }
        public string? Operacion { get; set; }
        public string? Descripcion { get; set; }
        public string? Vistas { get; set; }
        public string? Tipo { get; set; }
        public string? Nivel { get; set; }
        public string? Carpeta { get; set; }
        public string? FuenteDeDatos { get; set; }
        public string? TipoDestino { get; set; }
        public string? Grupo { get; set; }
        public string? Trans { get; set; }
        public string? Bloque { get; set; }
        public string? TipoDePerfil { get; set; }
        public string? Workflow { get; set; }
        public string? Workview { get; set; }
        public string? SubMenu { get; set; }
        
        public string? FuncionNivel1 { get; set; }
        public string? FuncionNivel2 { get; set; }
        public string? FuncionNivel3 { get; set; }
        public string? FuncionNivel4 { get; set; }
        public string? FuncionNivel5 { get; set; }
        public string? FuncionNivel6 { get; set; }
        public string? FuncionNivel7 { get; set; }
        
        public DateTime FechaCreacion { get; set; }
    }
}
