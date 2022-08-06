using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapie.WebUI.Models.DataContexts;

namespace Vapie.WebUI.AppCode.ViewComponents
{
    public class HeaderViewComponent : ViewComponent
    {
        private readonly VapieDbContext db;

        public HeaderViewComponent(VapieDbContext db)
        {
            this.db = db;
        }
        public IViewComponentResult Invoke()
        {
            var datas = db.Categories
                .Where(c => c.DeletedById == null)
                .ToList();

            ViewBag.Categories = datas;
            return View();
        }
    }
}
