using MatrizPerfiles.Web.Models;

namespace MatrizPerfiles.Web.Repositories
{
    public interface IMatrizPerfilRepository : IRepository<PbiMatrizPerfil>
    {
        Task<PbiMatrizPerfil?> GetByIdWithRelationsAsync(int id);
        Task<(IEnumerable<PbiMatrizPerfil> Data, int TotalRecords, int FilteredRecords)> GetPagedAsync(
            string? search, 
            int start, 
            int length, 
            string sortColumn, 
            string sortDirection,
            string? empresaFilter = null,
            int? sistemaFilter = null,
            int? puestoFilter = null,
            string? codigoFilter = null,
            string? funcionRolFilter = null,
            DateTime? startDate = null,
            DateTime? endDate = null);
        Task<int> GetTotalCountAsync();
    }
}
