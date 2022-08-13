using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Infrastructure;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;
using Vapie.WebUI.Models.ViewModels;

namespace Vapie.WebUI.Controllers
{
    [AllowAnonymous]
    public class ShopController : Controller
    {
        private readonly VapieDbContext db;

        public ShopController(VapieDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index(int pageIndex = 1, int pageSize = 10)
        {
            var datas = db.Products
                .Where(b => b.DeletedById == null)
                .Include(i => i.Images.Where(i => i.IsMain == true))
                .ToList();
            var pagedModel = new PagedViewModel<Product>(datas, pageIndex, pageSize);
            return View(pagedModel);
        }
        public IActionResult Category(int categoryId,int pageIndex = 1, int pageSize = 10)
        {
            var datas = db.Products
                .Where(b => b.DeletedById == null && b.CategoryId==categoryId)
                .Include(i => i.Images.Where(i => i.IsMain == true))
                .ToList();
            var pagedModel = new PagedViewModel<Product>(datas, pageIndex, pageSize);
            return View(pagedModel);
        }
        public IActionResult SingleProduct(int id,int categoryId)
        {
            var shopViewModel = new ShopViewModel();
            shopViewModel.Product = db.Products
                .Include(p => p.Images)
                .Include(p=>p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.DeletedById == null && p.Id == id);

            shopViewModel.Products = db.Products
                .Where(p => p.DeletedById == null && p.CategoryId==categoryId && p.Id !=id)
                .Include(p=>p.Images.Where(i => i.IsMain == true))
                .Include(p=>p.Brand)
                .ToList();

            
            //ViewBag.CategoryId = categoryId;

            return View(shopViewModel);
        }

        public async Task<IActionResult> SearchInput(string key)
        {
            List<Product> products = new List<Product>();
            if (key != null)
            {
                products = await db.Products
                .Where(p => p.Name.Contains(key)
                || p.Description.Contains(key)
                || p.Price.ToString().Contains(key)
                || p.Category.Name.Contains(key)
                || p.Brand.Name.Contains(key))
                .Include(p=>p.Images)
                .Include(p=>p.Category)
                .ToListAsync();
            }
            return PartialView("_ProductListPartial", products);
        }


        public IActionResult Basket()
        {
            if (Request.Cookies.TryGetValue("cards", out string cards))
            {
                int[] idsFromCookie = cards.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();

                var products = from p in db.Products.Include(p=>p.Images).Where(p => p.DeletedById == null)
                               where idsFromCookie.Contains(p.Id) && p.DeletedById == null
                               select p;

                return View(products.ToList());

            }

            return View(new List<Product>());

        }
        //public CommandJsonResponse Delete(int id)
        //{
        //    if (Request.Cookies.TryGetValue("cards", out string cards))
        //    {
        //        int[] idsFromCookie = cards.Split(",").Where(CheckIsNumber)
        //                .Select(item => int.Parse(item))
        //                .ToArray();


        //        idsFromCookie = idsFromCookie.Where(i => i != id).ToArray();
        //        int count = Request.Cookies["cards"].Values.Count;
        //        return new CommandJsonResponse(false, "Deleted Successfully");
        //    }
        //    return new CommandJsonResponse(true, "Qeyd Movcud Deyil!");
        //}

        private bool CheckIsNumber(string value)
        {
            return int.TryParse(value, out int v);
        }
    }
}
