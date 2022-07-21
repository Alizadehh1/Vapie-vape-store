using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Modules.BrandModule;
using Vapie.WebUI.Models.Entities;
using Vapie.WebUI.Models.ViewModels;

namespace Vapie.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly IMediator mediator;

        public BrandsController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<IActionResult> Index(BrandsAllQuery query, int pageIndex = 1, int pageSize = 4)
        {
            var data = await mediator.Send(query);

            var pagedModel = new PagedViewModel<Brand>(data, pageIndex, pageSize);

            return View(pagedModel);
        }
        public async Task<IActionResult> Details(BrandSingleQuery query)
        {
            var data = await mediator.Send(query);

            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(BrandCreateCommand command)
        {
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }
        public async Task<IActionResult> Edit(BrandSingleQuery query)
        {
            var entity = await mediator.Send(query);
            if (entity == null)
            {
                return NotFound();
            }
            var command = new BrandEditCommand();
            command.Id = entity.Id;
            command.Name = entity.Name;
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int id,BrandEditCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            if (ModelState.IsValid)
            {
                var response = await mediator.Send(command);
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(BrandRemoveCommand command)
        {
            var response = await mediator.Send(command);
            return Json(response);
        }
    }
}
