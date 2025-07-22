using AspNetApplication.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Shared.Events;
using Shared.Services.Abstraction;

namespace AspNetApplication.Controllers
{
    public class ProductController(IEventStoreService eventStoreService, IMongoDBService mongoDBService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var products = mongoDBService.GetCollection<Shared.Models.Product>("products");
            var productList = await (await products.FindAsync(p => true)).ToListAsync();
            return View(productList);
        }

        public IActionResult CreateProduct()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateEntity product)
        {
            NewProductEvent newProductEvent = new NewProductEvent
            {
                ProductId = Guid.NewGuid(),
                ProductName = product.Name,
                IsAvailable = product.IsAvailable,
                Price = product.Price
            };

            await eventStoreService.AppendToStreamAsync("product-stream", new[] { eventStoreService.NewEventData(newProductEvent) });
            return RedirectToAction("Index");
        }
    }
}

