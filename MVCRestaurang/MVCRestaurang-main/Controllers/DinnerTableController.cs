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

        // GET: DinnerTable/Index
        public async Task<IActionResult> Index()
        {
            var tables = await _dinnerTableService.GetDinnerTablesAsync();
            return View(tables);
        }

        // GET: DinnerTable/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var table = await _dinnerTableService.GetDinnerTableByIdAsync(id);
            if (table == null)
                return NotFound();

            return View(table);
        }

        // GET: DinnerTable/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DinnerTable/Create
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

        // GET: DinnerTable/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var table = await _dinnerTableService.GetDinnerTableByIdAsync(id);
            if (table == null)
                return NotFound();

            return View(table);
        }

        // POST: DinnerTable/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, DinnerTable table)
        {
            if (id != table.TableID)
            {
                return BadRequest("ID mismatch");
            }

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

        // POST: DinnerTable/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
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