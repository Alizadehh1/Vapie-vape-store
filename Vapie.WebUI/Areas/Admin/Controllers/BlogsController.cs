using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Vapie.WebUI.AppCode.Modules.BlogModule;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;
using Vapie.WebUI.Models.ViewModels;

namespace Vapie.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {
        private readonly VapieDbContext db;
        private readonly IMediator mediator;

        public BlogsController(VapieDbContext db, IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        public async Task<IActionResult> Index(BlogAllQuery query, int pageIndex = 1, int pageSize = 10)
        {
            var entity = await mediator.Send(query);

            var pagedModel = new PagedViewModel<Blog>(entity, pageIndex, pageSize);

            return View(pagedModel);
        }

        public async Task<IActionResult> Details(BlogSingleQuery query)
        {
            var blog = await mediator.Send(query);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateCommand command)
        {
            var response = await mediator.Send(command);
            if (ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", command.CategoryId);
            return View(command);
        }

        public async Task<IActionResult> Edit(BlogSingleQuery query)
        {
            var blog = await mediator.Send(query);
            if (blog == null)
            {
                return NotFound();
            }
            var command = new BlogEditCommand();
            command.Id = blog.Id;
            command.Title = blog.Title;
            command.Paragraph = blog.Paragraph;
            command.ImagePath = blog.ImagePath;
            command.CategoryId = blog.CategoryId;
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", blog.CategoryId);
            return View(command);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, BlogEditCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            await mediator.Send(command);
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", command.CategoryId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(BlogRemoveCommand command)
        {
            var response = await mediator.Send(command);
            return Json(response);
        }
    }
}
