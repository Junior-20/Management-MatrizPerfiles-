using MatrizPerfiles.Web.Data;
using MatrizPerfiles.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace MatrizPerfiles.Web.Repositories
{
    public class MatrizPerfilRepository : Repository<PbiMatrizPerfil>, IMatrizPerfilRepository
    {
        public MatrizPerfilRepository(MatrizPerfilesContext context) : base(context)
        {
        }

        public async Task<PbiMatrizPerfil?> GetByIdWithRelationsAsync(int id)
        {
            return await _context.PbiMatrizPerfiles
                .Include(p => p.Sistema)
                .Include(p => p.Puesto)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.PbiMatrizPerfiles.CountAsync();
        }

        public async Task<(IEnumerable<PbiMatrizPerfil> Data, int TotalRecords, int FilteredRecords)> GetPagedAsync(
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
            DateTime? endDate = null)
        {
            var query = _context.PbiMatrizPerfiles
                .Include(p => p.Sistema)
                .Include(p => p.Puesto)
                .AsNoTracking()
                .AsQueryable();

            int totalRecords = await query.CountAsync();

            // Advanced Filters
            if (!string.IsNullOrEmpty(empresaFilter))
                query = query.Where(p => p.Empresa != null && p.Empresa.Contains(empresaFilter));
            
            if (sistemaFilter.HasValue)
                query = query.Where(p => p.SistemaId == sistemaFilter.Value);

            if (puestoFilter.HasValue)
                query = query.Where(p => p.PuestoId == puestoFilter.Value);

            if (!string.IsNullOrEmpty(codigoFilter))
                query = query.Where(p => p.Codigo != null && p.Codigo.Contains(codigoFilter));

            if (!string.IsNullOrEmpty(funcionRolFilter))
                query = query.Where(p => p.FuncionRolAsociado != null && p.FuncionRolAsociado.Contains(funcionRolFilter));

            if (startDate.HasValue)
                query = query.Where(p => p.FechaCreacion >= startDate.Value);

            if (endDate.HasValue)
            {
                var endOfDate = endDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(p => p.FechaCreacion <= endOfDate);
            }

            // Global DataTables Search
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => 
                    (p.Codigo != null && p.Codigo.Contains(search)) ||
                    (p.Empresa != null && p.Empresa.Contains(search)) ||
                    (p.Modulo != null && p.Modulo.Contains(search)) ||
                    (p.Sistema != null && p.Sistema.Nombre.Contains(search)) ||
                    (p.Puesto != null && p.Puesto.Nombre.Contains(search))
                );
            }

            int filteredRecords = await query.CountAsync();

            // Sorter
            bool isAsc = sortDirection.ToLower() == "asc";
            
            query = sortColumn switch
            {
                "Empresa" => isAsc ? query.OrderBy(p => p.Empresa) : query.OrderByDescending(p => p.Empresa),
                "Sistema.Nombre" => isAsc ? query.OrderBy(p => p.Sistema!.Nombre) : query.OrderByDescending(p => p.Sistema!.Nombre),
                "Puesto.Nombre" => isAsc ? query.OrderBy(p => p.Puesto!.Nombre) : query.OrderByDescending(p => p.Puesto!.Nombre),
                "Codigo" => isAsc ? query.OrderBy(p => p.Codigo) : query.OrderByDescending(p => p.Codigo),
                "FuncionRolAsociado" => isAsc ? query.OrderBy(p => p.FuncionRolAsociado) : query.OrderByDescending(p => p.FuncionRolAsociado),
                "FechaCreacion" => isAsc ? query.OrderBy(p => p.FechaCreacion) : query.OrderByDescending(p => p.FechaCreacion),
                _ => isAsc ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id)
            };

            // Paging
            var data = await query.Skip(start).Take(length).ToListAsync();

            return (data, totalRecords, filteredRecords);
        }
    }
}
