using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Extensions;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Modules.ProductModule
{
    public class ProductCreateCommand : IRequest<Product>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string Capacity { get; set; }
        public string Size { get; set; }
        public string Flavor { get; set; }
        public string NicotineStrength { get; set; }
        public double Price { get; set; }
        public double? OldPrice { get; set; }
        public int isMainIndex { get; set; }
        public ImageItem[] Images { get; set; }
        [Obsolete]
        public class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, Product>
        {
            readonly VapieDbContext db;
            readonly IActionContextAccessor ctx;
            readonly IHostingEnvironment env;

            public ProductCreateCommandHandler(VapieDbContext db,
                IActionContextAccessor ctx,
                IHostingEnvironment env)
            {
                this.db = db;
                this.ctx = ctx;
                this.env = env;
            }

            public async Task<Product> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
            {



                var product = new Product();

                product.Name = request.Name;
                product.CategoryId = request.CategoryId;
                product.BrandId = request.BrandId;
                product.Description = request.Description;
                product.Capacity = request.Capacity;
                product.Flavor = request.Flavor;
                product.NicotineStrength = request.NicotineStrength;
                product.Price = request.Price;
                product.OldPrice = request.OldPrice;

                int count = 0;
                if (request.Images != null && request.Images.Any(i => i.File != null))
                {
                    product.Images = new List<ProductImage>();

                    foreach (var productFile in request.Images.Where(i => i.File != null))
                    {
                        string name = await env.SaveFile(productFile.File, cancellationToken, "product");
                        if (count==request.isMainIndex)
                        {
                            product.Images.Add(new ProductImage
                            {
                                ImagePath = name,
                                IsMain = true
                            });
                        }
                        else
                        {
                            product.Images.Add(new ProductImage
                            {
                                ImagePath = name,
                                IsMain = false
                            });
                        }
                        
                        count++;

                    }
                }
                else
                {
                    ctx.AddModelError("Images", "Sekil qeyd edilmeyib");
                    goto l1;
                }



                await db.Products.AddAsync(product, cancellationToken);
                try
                {
                    await db.SaveChangesAsync(cancellationToken);
                    return product;
                }
                catch (Exception ex)
                {

                    ctx.AddModelError("Name", "Xeta bash verdi,Biraz sonra yeniden yoxlayin");

                    return product;
                }

            l1:
                return null;
            }
        }
    }
}
