using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Extensions;
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
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 12)
        {
            var viewModel = new ShopViewModel();
            var query = db.Products
                .Where(b => b.DeletedById == null)
                .Include(i => i.Images.Where(i => i.IsMain == true))
                .ToList();
            viewModel.Categories = await db.Categories
                .Where(c => c.DeletedById == null)
                .Include(c => c.Children.Where(c => c.DeletedById == null))
                .ToListAsync();

            viewModel.PagedViewModel = new PagedViewModel<Product>(query, pageIndex, pageSize);
            return View(viewModel);
        }
        public async Task<IActionResult> Category(int categoryId, int pageIndex = 1, int pageSize = 12)
        {
            var viewModel = new ShopViewModel();
            var query = db.Products
                .Where(b => b.DeletedById == null && b.CategoryId == categoryId)
                .Include(p => p.Category)
                .Include(i => i.Images.Where(i => i.IsMain == true))
                .ToList();
            viewModel.Categories = await db.Categories
                .Where(c => c.DeletedById == null)
                .Include(c => c.Children.Where(c => c.DeletedById == null))
                .ToListAsync();
            if (query.Count > 0)
            {
                viewModel.PagedViewModel = new PagedViewModel<Product>(query, pageIndex, pageSize);
                return View(viewModel);
            }
            else
            {
                return View(null);
            }
        }
        public IActionResult SingleProduct(int id, int categoryId)
        {
            var shopViewModel = new ShopViewModel();
            shopViewModel.Product = db.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .FirstOrDefault(p => p.DeletedById == null && p.Id == id);

            shopViewModel.Products = db.Products
                .Where(p => p.DeletedById == null && p.CategoryId == categoryId && p.Id != id)
                .Include(p => p.Images.Where(i => i.IsMain == true))
                .Include(p => p.Brand)
                .ToList();

            shopViewModel.Comments = db.ProductComments
                .Where(c => c.DeletedById == null && c.ProductId == id)
                .ToList();


            return View(shopViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Comment(ProductComment model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    error = true,
                    message = ModelState.SelectMany(ms => ms.Value.Errors).First().ErrorMessage
                });
            }
            await db.ProductComments.AddAsync(model);
            await db.SaveChangesAsync();
            return Json(new
            {
                error = false,
                message = "Muracietiniz qeyde alindi!"
            });
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
                .Include(p => p.Images)
                .Include(p => p.Category)
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

                var products = from p in db.Products.Include(p => p.Images).Where(p => p.DeletedById == null)
                               where idsFromCookie.Contains(p.Id) && p.DeletedById == null
                               select p;

                return View(products.ToList());

            }

            return View(new List<Product>());

        }
        public IActionResult Wishlist()
        {
            if (Request.Cookies.TryGetValue("cardsForCookie", out string cardsForCookie))
            {
                int[] idsFromCookie = cardsForCookie.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();

                var products = from p in db.Products.Include(p => p.Images).Where(p => p.DeletedById == null)
                               where idsFromCookie.Contains(p.Id) && p.DeletedById == null
                               select p;

                return View(products.ToList());

            }

            return View(new List<Product>());

        }

        public async Task<IActionResult> PlaceOrder(string productIds, string totalAmount, string quantities, string prices)
        {
            int[] productId = productIds.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();
            int[] quantity = quantities.Split(",").Where(CheckIsNumber)
                        .Select(item => int.Parse(item))
                        .ToArray();

            double[] price = prices.Split(",").Where(CheckIsDouble)
                        .Select(item => double.Parse(item))
                        .ToArray();


            var newOrder = new Order();
            newOrder.VapieUserId = User.GetUserId();
            newOrder.TotalAmount = Convert.ToDouble(totalAmount);
            if (productId != null)
            {
                newOrder.OrderItems = new List<OrderItem>();
                int i = 0;
                foreach (var id in productId)
                {

                    newOrder.OrderItems.Add(new OrderItem
                    {
                        ProductId = id,
                        OrderId = newOrder.Id,
                        Quantity = quantity[i],
                        Price = price[i]
                    });
                    i++;
                }
            }
            await db.Orders.AddAsync(newOrder);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool CheckIsNumber(string value)
        {
            return int.TryParse(value, out int v);
        }
        private bool CheckIsDouble(string value)
        {
            return double.TryParse(value, out double v);
        }
    }
}
