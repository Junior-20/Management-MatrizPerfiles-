using MatrizPerfiles.Web.Models;
using MatrizPerfiles.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MatrizPerfiles.Web.Controllers
{
    public class PuestosController : Controller
    {
        private readonly IRepository<Puesto> _repository;

        public PuestosController(IRepository<Puesto> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var puestos = await _repository.GetAllAsync();
            return View(puestos);
        }

        public IActionResult Create()
        {
            return View(new Puesto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Puesto puesto)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(puesto);
                TempData["SuccessMessage"] = "Puesto creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(puesto);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var puesto = await _repository.GetByIdAsync(id);
            if (puesto == null) return NotFound();
            return View(puesto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Puesto puesto)
        {
            if (id != puesto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(puesto);
                TempData["SuccessMessage"] = "Puesto actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(puesto);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var puesto = await _repository.GetByIdAsync(id);
            if (puesto == null)
            {
                return Json(new { success = false, message = "Puesto no encontrado." });
            }

            try
            {
                await _repository.DeleteAsync(puesto);
                return Json(new { success = true, message = "Puesto eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                // In production, log exception
                return Json(new { success = false, message = "No se puede eliminar el puesto porque está en uso o ocurrió un error." });
            }
        }
    }
}
