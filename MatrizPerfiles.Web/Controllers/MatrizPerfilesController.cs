using MatrizPerfiles.Web.Models;
using MatrizPerfiles.Web.Repositories;
using MatrizPerfiles.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MatrizPerfiles.Web.Controllers
{
    public class MatrizPerfilesController : Controller
    {
        private readonly IMatrizPerfilRepository _repository;
        private readonly IRepository<Sistema> _sistemaRepository;
        private readonly IRepository<Puesto> _puestoRepository;

        public MatrizPerfilesController(
            IMatrizPerfilRepository repository,
            IRepository<Sistema> sistemaRepository,
            IRepository<Puesto> puestoRepository)
        {
            _repository = repository;
            _sistemaRepository = sistemaRepository;
            _puestoRepository = puestoRepository;
        }

        public async Task<IActionResult> Index()
        {
            // Ppopulate Dashboard stats & filters data
            ViewBag.SistemasCount = (await _sistemaRepository.GetAllAsync()).Count();
            ViewBag.PuestosCount = (await _puestoRepository.GetAllAsync()).Count();
            ViewBag.TotalRecords = await _repository.GetTotalCountAsync();
            
            ViewBag.Sistemas = new SelectList(await _sistemaRepository.GetAllAsync(), "Id", "Nombre");
            ViewBag.Puestos = new SelectList(await _puestoRepository.GetAllAsync(), "Id", "Nombre");

            return View();
        }

        // DataTables Endpoint Server-side rendering
        [HttpPost]
        public async Task<IActionResult> GetDataTableData()
        {
            try 
            {
                var draw = Request.Form["draw"].FirstOrDefault();
                var start = Request.Form["start"].FirstOrDefault();
                var length = Request.Form["length"].FirstOrDefault();
                var sortColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
                var sortColumn = Request.Form[$"columns[{sortColumnIndex}][name]"].FirstOrDefault();
                if (string.IsNullOrEmpty(sortColumn)) sortColumn = "Id";
                var sortDirection = Request.Form["order[0][dir]"].FirstOrDefault() ?? "asc";
                var searchValue = Request.Form["search[value]"].FirstOrDefault();

                // Custom advanced filters from DataTables payload
                var empresaFilter = Request.Form["empresa"].FirstOrDefault();
                var sistemaFilterRaw = Request.Form["sistemaId"].FirstOrDefault();
                var puestoFilterRaw = Request.Form["puestoId"].FirstOrDefault();
                var codigoFilter = Request.Form["codigo"].FirstOrDefault();
                var funcionRolFilter = Request.Form["funcionRolAsociado"].FirstOrDefault();
                var startDateRaw = Request.Form["startDate"].FirstOrDefault();
                var endDateRaw = Request.Form["endDate"].FirstOrDefault();

                int pageSize = length != null ? Convert.ToInt32(length) : 10;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                
                int? sistemaId = string.IsNullOrEmpty(sistemaFilterRaw) ? null : Convert.ToInt32(sistemaFilterRaw);
                int? puestoId = string.IsNullOrEmpty(puestoFilterRaw) ? null : Convert.ToInt32(puestoFilterRaw);
                
                DateTime? startDate = string.IsNullOrEmpty(startDateRaw) ? null : Convert.ToDateTime(startDateRaw);
                DateTime? endDate = string.IsNullOrEmpty(endDateRaw) ? null : Convert.ToDateTime(endDateRaw);

                var result = await _repository.GetPagedAsync(
                    searchValue, skip, pageSize, sortColumn, sortDirection,
                    empresaFilter, sistemaId, puestoId, codigoFilter, funcionRolFilter, startDate, endDate);

                // Format data for DataTables
                var jsonData = new
                {
                    draw = draw,
                    recordsTotal = result.TotalRecords,
                    recordsFiltered = result.FilteredRecords,
                    data = result.Data.Select(p => new
                    {
                        p.Id,
                        Empresa = p.Empresa ?? "-",
                        DescripcionUnidadOrganizativa = p.DescripcionUnidadOrganizativa ?? "-",
                        Codigo = p.Codigo ?? "-",
                        SistemaNombre = p.Sistema?.Nombre ?? "-",
                        PuestoNombre = p.Puesto?.Nombre ?? "-",
                        Modulo = p.Modulo ?? "-",
                        SubModulo = p.SubModulo ?? "-",
                        Menu = p.Menu ?? "-",
                        Sitio = p.Sitio ?? "-",
                        SubSitio = p.SubSitio ?? "-",
                        OpcionDeMenu = p.OpcionDeMenu ?? "-",
                        Opcion = p.Opcion ?? "-",
                        Programa = p.Programa ?? "-",
                        ArchivoDePrograma = p.ArchivoDePrograma ?? "-",
                        FuncionRolAsociado = p.FuncionRolAsociado ?? "-",
                        CodigoIdFuncion = p.CodigoIdFuncion ?? "-",
                        Operacion = p.Operacion ?? "-",
                        Descripcion = p.Descripcion ?? "-",
                        Vistas = p.Vistas ?? "-",
                        Tipo = p.Tipo ?? "-",
                        Nivel = p.Nivel ?? "-",
                        Carpeta = p.Carpeta ?? "-",
                        FuenteDeDatos = p.FuenteDeDatos ?? "-",
                        TipoDestino = p.TipoDestino ?? "-",
                        Grupo = p.Grupo ?? "-",
                        Trans = p.Trans ?? "-",
                        Bloque = p.Bloque ?? "-",
                        TipoDePerfil = p.TipoDePerfil ?? "-",
                        Workflow = p.Workflow ?? "-",
                        Workview = p.Workview ?? "-",
                        SubMenu = p.SubMenu ?? "-",
                        FuncionNivel1 = p.FuncionNivel1 ?? "-",
                        FuncionNivel2 = p.FuncionNivel2 ?? "-",
                        FuncionNivel3 = p.FuncionNivel3 ?? "-",
                        FuncionNivel4 = p.FuncionNivel4 ?? "-",
                        FuncionNivel5 = p.FuncionNivel5 ?? "-",
                        FuncionNivel6 = p.FuncionNivel6 ?? "-",
                        FuncionNivel7 = p.FuncionNivel7 ?? "-",
                        FechaCreacion = p.FechaCreacion.ToString("dd/MM/yyyy HH:mm")
                    })
                };

                return Ok(jsonData);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var matrizPerfil = await _repository.GetByIdWithRelationsAsync(id);
            if (matrizPerfil == null) return NotFound();
            return View(matrizPerfil);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new MatrizPerfilViewModel
            {
                MatrizPerfil = new PbiMatrizPerfil(),
                ListaSistemas = new SelectList(await _sistemaRepository.GetAllAsync(), "Id", "Nombre"),
                ListaPuestos = new SelectList(await _puestoRepository.GetAllAsync(), "Id", "Nombre")
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MatrizPerfilViewModel viewModel)
        {
            // Remove navigation properties Validation
            ModelState.Remove("MatrizPerfil.Sistema");
            ModelState.Remove("MatrizPerfil.Puesto");
            ModelState.Remove("ListaSistemas");
            ModelState.Remove("ListaPuestos");

            if (ModelState.IsValid)
            {
                await _repository.AddAsync(viewModel.MatrizPerfil);
                TempData["SuccessMessage"] = "Registro de Matriz creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            viewModel.ListaSistemas = new SelectList(await _sistemaRepository.GetAllAsync(), "Id", "Nombre");
            viewModel.ListaPuestos = new SelectList(await _puestoRepository.GetAllAsync(), "Id", "Nombre");
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var matrizPerfil = await _repository.GetByIdAsync(id);
            if (matrizPerfil == null) return NotFound();

            var viewModel = new MatrizPerfilViewModel
            {
                MatrizPerfil = matrizPerfil,
                ListaSistemas = new SelectList(await _sistemaRepository.GetAllAsync(), "Id", "Nombre"),
                ListaPuestos = new SelectList(await _puestoRepository.GetAllAsync(), "Id", "Nombre")
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MatrizPerfilViewModel viewModel)
        {
            if (id != viewModel.MatrizPerfil.Id) return NotFound();

            ModelState.Remove("MatrizPerfil.Sistema");
            ModelState.Remove("MatrizPerfil.Puesto");
            ModelState.Remove("ListaSistemas");
            ModelState.Remove("ListaPuestos");

            if (ModelState.IsValid)
            {
                // Do not update FechaCreacion, so we retrieve the existing to preserve it
                var existing = await _repository.GetByIdAsync(id);
                if(existing == null) return NotFound();

                // Simple property update mapping could be used here (e.g. Automapper). For brevity we update properties directly or context Update
                viewModel.MatrizPerfil.FechaCreacion = existing.FechaCreacion;
                
                // Detach existing before updating with new entity
                // This is a quick workaround for Entity tracking in generic repos. 
                // A better approach is copying properties, but we'll use EF's Update
                _repository.GetType().GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                    .GetValue(_repository);

                await _repository.UpdateAsync(viewModel.MatrizPerfil);
                TempData["SuccessMessage"] = "Registro de Matriz actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            viewModel.ListaSistemas = new SelectList(await _sistemaRepository.GetAllAsync(), "Id", "Nombre");
            viewModel.ListaPuestos = new SelectList(await _puestoRepository.GetAllAsync(), "Id", "Nombre");
            return View(viewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var matrizPerfil = await _repository.GetByIdAsync(id);
            if (matrizPerfil == null)
            {
                return Json(new { success = false, message = "Registro no encontrado." });
            }

            try
            {
                await _repository.DeleteAsync(matrizPerfil);
                return Json(new { success = true, message = "Registro eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Ocurrió un error al intentar eliminar el registro." });
            }
        }
    }
}
