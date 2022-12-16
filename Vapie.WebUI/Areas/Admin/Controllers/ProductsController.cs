using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vapie.WebUI.AppCode.Infrastructure;
using Vapie.WebUI.AppCode.Modules.ProductModule;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;
using Vapie.WebUI.Models.ViewModels;

namespace Vapie.WebUI.Areas.Admin.Controllers
{
    [AllowAnonymous]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly VapieDbContext db;
        private readonly IMediator mediator;

        public ProductsController(VapieDbContext db,IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index(ProductAllQuery query)
        {
            var data = await mediator.Send(query);


            return View(data);
        }

        public async Task<IActionResult> Details(ProductSingleQuery query)
        {
            var entity = await mediator.Send(query);

            if (entity == null)
            {
                return NotFound();
            }


            return View(entity);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name");
            ViewData["BrandId"] = new SelectList(db.Brands, "Id", "Name");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateCommand command)
        {
            var product = await mediator.Send(command);

            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(command);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products
                .Include(p=>p.Category)
                .Include(p=>p.Brand)
                .Include(p=>p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewData["BrandId"] = new SelectList(db.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductEditCommand model)
        {
            var product = await mediator.Send(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete([FromRoute] int id)
        {
            var entity = db.Products.FirstOrDefault(c => c.Id == id);
            if (entity == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Movcud deyil"
                });
            }
            db.Products.Remove(entity);
            db.SaveChanges();
            return Json(new
            {
                error = false,
                message = "Ugurla silindi"
            });
        }


        private bool ProductExists(int id)
        {
            return db.Products.Any(e => e.Id == id);
        }
    }
}
