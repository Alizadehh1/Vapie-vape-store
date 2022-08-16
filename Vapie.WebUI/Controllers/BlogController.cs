using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;
using Vapie.WebUI.Models.ViewModels;

namespace Vapie.WebUI.Controllers
{
    [AllowAnonymous]
    public class BlogController : Controller
    {
        private readonly VapieDbContext db;

        public BlogController(VapieDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index(int pageIndex = 1, int pageSize = 10)
        {
            var datas = db.Blogs
                .Where(b => b.DeletedById == null)
                .ToList();
            var pagedModel = new PagedViewModel<Blog>(datas, pageIndex, pageSize);
            return View(pagedModel);
        }

        public IActionResult Category(int categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var datas = db.Blogs
                .Where(b => b.DeletedById == null && b.CategoryId == categoryId)
                .Include(p => p.Category)
                .ToList();
            var pagedModel = new PagedViewModel<Blog>(datas, pageIndex, pageSize);
            return View(pagedModel);
        }

        public IActionResult SinglePost(int id,int categoryId)
        {
            var data = db.Blogs
                .Where(b => b.DeletedById == null)
                .ToList();
            
            ViewBag.BlogId = id;
            ViewBag.CategoryId = categoryId;
            return View(data);
        }
    }
}
