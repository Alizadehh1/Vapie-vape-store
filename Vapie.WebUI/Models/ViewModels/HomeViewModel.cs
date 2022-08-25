using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Product> Products { get; set; }
        public List<Product> FeaturedProducts { get; set; }
        public List<Product> TopSellingProducts { get; set; }
        public List<Slider> Sliders { get; set; }
    }
}
