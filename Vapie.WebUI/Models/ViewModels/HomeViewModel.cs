using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> LastestProducts { get; set; }
        public List<Product> SaleProducts { get; set; }
    }
}
