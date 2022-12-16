using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;
using Vapie.WebUI.Models.Entities.Membership;
using Vapie.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Vapie.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly VapieDbContext db;
        private readonly UserManager<VapieUser> userManager;

        public OrdersController(VapieDbContext db, UserManager<VapieUser> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }
        [Authorize(Policy = "admin.orders.index")]
        public IActionResult Index(int pageIndex = 1, int pageSize = 10)
        {
            var viewModel = new OrderViewModel();
            var query = db.Orders
                .OrderByDescending(o=>o.CreatedDate)
                .Include(o => o.OrderItems);
            viewModel.Users = db.Users.ToList();
            viewModel.PagedViewModel = new PagedViewModel<Order>(query, pageIndex, pageSize);
            return View(viewModel);
        }
        [Authorize(Policy = "admin.orders.details")]
        public IActionResult Details(int id)
        {
            var viewModel = new OrderViewModel();
            viewModel.Order = db.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi=>oi.Product)
                .FirstOrDefault(o => o.Id == id);
            viewModel.Orders = db.Orders
                .Where(o => o.Id == id)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .ToList();
            viewModel.Products = db.Products
                .Where(p => p.DeletedById == null)
                .Include(p=>p.Category)
                .Include(p=>p.Brand)
                .Include(p=>p.Images.Where(i=>i.IsMain==true && i.DeletedById==null))
                .ToList();
            viewModel.Users = db.Users.ToList();
            return View(viewModel);
        }
        [HttpPost]
        [Authorize(Policy = "admin.orders.confirm")]
        public IActionResult Confirm([FromRoute]int id)
        {
            var entity = db.Orders.Include(o=>o.OrderItems).FirstOrDefault(b => b.Id == id && b.DeletedById == null);
            if (entity == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Movcud deyil"
                });
            }
            entity.isConfirmed = true;
            var orderItems = entity.OrderItems.Where(oi => oi.OrderId == entity.Id);
            foreach (var orderItem in orderItems)
            {
                var product = db.Products.FirstOrDefault(p => p.Id == orderItem.ProductId);
                //product.Quantity = product.Quantity - orderItem.Quantity;
            }
            db.SaveChanges();
            return Json(new
            {
                error = false,
                message = "Ugurla Təsdiqləndi"
            });
        }
        [HttpPost]
        [Authorize(Policy = "admin.orders.cancel")]
        public async Task<IActionResult> Cancel([FromRoute]int id)
        {
            var entity = db.Orders.FirstOrDefault(b => b.Id == id && b.isConfirmed == false);
            if (entity == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Movcud deyil"
                });
            }
            var user = await userManager.GetUserAsync(User);
            entity.DeletedById = user.Id;
            entity.DeletedDate = DateTime.UtcNow.AddHours(4);
            db.SaveChanges();
            db.SaveChanges();
            return Json(new
            {
                error = false,
                message = "Ugurla Təsdiqləndi"
            });
        }
    }
}
