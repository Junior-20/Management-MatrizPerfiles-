using MatrizPerfiles.Web.Models;
using MatrizPerfiles.Web.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MatrizPerfiles.Web.Controllers
{
    public class SistemasController : Controller
    {
        private readonly IRepository<Sistema> _repository;

        public SistemasController(IRepository<Sistema> repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var sistemas = await _repository.GetAllAsync();
            return View(sistemas);
        }

        public IActionResult Create()
        {
            return View(new Sistema());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Sistema sistema)
        {
            if (ModelState.IsValid)
            {
                await _repository.AddAsync(sistema);
                TempData["SuccessMessage"] = "Sistema creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(sistema);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var sistema = await _repository.GetByIdAsync(id);
            if (sistema == null) return NotFound();
            return View(sistema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Sistema sistema)
        {
            if (id != sistema.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _repository.UpdateAsync(sistema);
                TempData["SuccessMessage"] = "Sistema actualizado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            return View(sistema);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var sistema = await _repository.GetByIdAsync(id);
            if (sistema == null)
            {
                return Json(new { success = false, message = "Sistema no encontrado." });
            }

            try
            {
                await _repository.DeleteAsync(sistema);
                return Json(new { success = true, message = "Sistema eliminado exitosamente." });
            }
            catch (Exception ex)
            {
                // In production, log exception
                return Json(new { success = false, message = "No se puede eliminar el sistema porque está en uso o ocurrió un error." });
            }
        }
    }
}
