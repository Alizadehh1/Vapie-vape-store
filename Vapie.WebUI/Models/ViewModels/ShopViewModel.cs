using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.Models.ViewModels
{
    public class ShopViewModel
    {
        public List<Category> Categories { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Product> Products { get; set; }
        public Product Product { get; set; }
        public ProductComment Comment { get; set; }
        public List<ProductComment> Comments { get; set; }
        public PagedViewModel<Product> PagedViewModel { get; set; }
    }
}
