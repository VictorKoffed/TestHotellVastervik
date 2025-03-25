using Microsoft.AspNetCore.Mvc;
using restaurangprojekt.Services;
using restaurangprojekt.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Text;

namespace restaurangprojekt.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        // Dependency Injection av ProductService
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // ✅ Visa alla produkter
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        // ✅ Visa detaljer för en specifik produkt
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // ✅ Visa formulär för att skapa en produkt
        public IActionResult Create()
        {
            return View();
        }

        // ✅ POST: Skapa en ny produkt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var createdProduct = await _productService.CreateProductAsync(product);
                if (createdProduct != null)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Kunde inte skapa produkten.");
            }
            return View(product);
        }

        // ✅ Visa formulär för att redigera en produkt
        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // ✅ POST: Uppdatera en produkt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductID)
                return BadRequest();

            if (ModelState.IsValid)
            {
                var success = await _productService.UpdateProductAsync(id, product);
                if (success)
                    return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", "Kunde inte uppdatera produkten.");
            }

            return View(product);
        }

        // ✅ Visa bekräftelse på att ta bort produkt
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // ✅ POST: Ta bort produkten
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _productService.DeleteProductAsync(id);
            if (deleted)
                return RedirectToAction(nameof(Index));

            return BadRequest();
        }

        // 🔸 Visar endast produkter från kategorin "Dryckesmeny"
        public async Task<IActionResult> Dryckesmeny()
        {
            var drinks = await _productService.GetDryckesmenyAsync();
            return View(drinks);
        }
        // 🔸 Visar endast produkter från kategorin "Förrätt"
        public async Task<IActionResult> Forratter()
        {
            var forratter = await _productService.GetForratterAsync();
            return View(forratter);
        }

        // 🔸 Visar endast produkter från kategorin "Varmrätt"
        public async Task<IActionResult> Varmratter()
        {
            var varmratter = await _productService.GetVarmratterAsync();
            return View(varmratter);
        }

        // 🔸 Visar endast produkter från kategorin "Dessert"
        public async Task<IActionResult> Desserter()
        {
            var desserter = await _productService.GetDesserterAsync();
            return View(desserter);
        }

        // 🔸 Visar alla menyprodukter samlat
        public async Task<IActionResult> Menu()
        {
            ViewBag.Forratter = await _productService.GetForratterAsync();
            ViewBag.Varmratter = await _productService.GetVarmratterAsync();
            ViewBag.Desserter = await _productService.GetDesserterAsync();
            ViewBag.Drycker = await _productService.GetDryckesmenyAsync();

            return View();
        }
    }

}