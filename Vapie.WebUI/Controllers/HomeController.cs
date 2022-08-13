using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Modules.SubscribeModule;
using Vapie.WebUI.Models;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly VapieDbContext db;
        private readonly IMediator mediator;

        public HomeController(VapieDbContext db,IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Preloader()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Contact(Contact model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    error = true,
                    message = ModelState.SelectMany(ms => ms.Value.Errors).First().ErrorMessage
                });
            }
            await db.Contacts.AddAsync(model);
            await db.SaveChangesAsync();
            return Json(new
            {
                error = false,
                message = "Muracietiniz qeyde alindi!"
            });
        }
        [HttpPost]
        public async Task<IActionResult> Subscribe(SubscribeCreateCommand command)
        {

            var response = await mediator.Send(command);

            return Json(response);
        }

        [HttpGet]
        [Route("/subscribe-confirm")]
        public async Task<IActionResult> SubscribeConfirm(SubscribeConfirmCommand command)
        {
            var response = await mediator.Send(command);

            return View(response);
        }

        public IActionResult Faqs()
        {
            var faqs = db.Faqs.ToList();
            return View(faqs);
        }
    }
}
