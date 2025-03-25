using Microsoft.AspNetCore.Mvc;
using restaurangprojekt.Services;
using restaurangprojekt.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace restaurangprojekt.Controllers
{
    public class DinnerTableController : Controller
    {
        private readonly DinnerTableService _dinnerTableService;

        public DinnerTableController(DinnerTableService dinnerTableService)
        {
            _dinnerTableService = dinnerTableService;
        }

        public async Task<IActionResult> Index()
        {
            var tables = await _dinnerTableService.GetDinnerTablesAsync();
            return View(tables);
        }

        public async Task<IActionResult> Details(int id)
        {
            var table = await _dinnerTableService.GetDinnerTableByIdAsync(id);
            if (table == null)
                return NotFound();

            return View(table);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DinnerTable table)
        {
            if (!ModelState.IsValid)
                return View(table);

            var createdTable = await _dinnerTableService.CreateTableAsync(table);
            if (createdTable == null)
            {
                ModelState.AddModelError("", "Kunde inte skapa bord.");
                return View(table);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var table = await _dinnerTableService.GetDinnerTableByIdAsync(id);
            if (table == null)
                return NotFound();

            return View(table);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DinnerTable table)
        {
            if (!ModelState.IsValid)
                return View(table);

            var success = await _dinnerTableService.UpdateTableAsync(id, table);
            if (!success)
            {
                ModelState.AddModelError("", "Kunde inte uppdatera bord.");
                return View(table);
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var table = await _dinnerTableService.GetDinnerTableByIdAsync(id);
            if (table == null)
                return NotFound();

            return View(table);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _dinnerTableService.DeleteTableAsync(id);
            if (!success)
            {
                return Problem("Kunde inte radera bord.");
            }

            return RedirectToAction(nameof(Index));
        }
    }
}