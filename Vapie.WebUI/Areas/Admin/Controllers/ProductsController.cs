using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> Index(ProductAllQuery query, int pageIndex = 1, int pageSize = 10)
        {
            var data = await mediator.Send(query);

            var pagedModel = new PagedViewModel<Product>(data, pageIndex, pageSize);

            return View(pagedModel);
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
            var response = await mediator.Send(command);
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", command.CategoryId);
            ViewData["BrandId"] = new SelectList(db.Brands, "Id", "Name", command.BrandId);
            return View(command);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Name,Description,CategoryId,Capacity,Size,Flavor,NicotineStrength,Price,Id,CreatedById,CreatedDate,DeletedById,DeletedDate")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(product);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            ViewData["BrandId"] = new SelectList(db.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }

        

        private bool ProductExists(int id)
        {
            return db.Products.Any(e => e.Id == id);
        }
    }
}
