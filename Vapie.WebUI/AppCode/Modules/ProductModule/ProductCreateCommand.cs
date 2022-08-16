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
    public class ProductCreateCommand : IRequest<ProductCreateCommandResponse>
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
        public ImageItem[] Images { get; set; }
        public class ProductCreateCommandHandler : IRequestHandler<ProductCreateCommand, ProductCreateCommandResponse>
        {
            readonly VapieDbContext db;
            readonly IActionContextAccessor ctx;
            readonly IWebHostEnvironment env;

            public ProductCreateCommandHandler(VapieDbContext db,
                IActionContextAccessor ctx,
                IWebHostEnvironment env)
            {
                this.db = db;
                this.ctx = ctx;
                this.env = env;
            }

            public async Task<ProductCreateCommandResponse> Handle(ProductCreateCommand request, CancellationToken cancellationToken)
            {

                    
                if (ctx.ModelIsValid())
                {
                    var response = new ProductCreateCommandResponse
                    {
                        Product = null,
                    };
                    return response;
                }

                var product = new Product();

                product.Name = request.Name;
                product.CategoryId = request.CategoryId;
                product.BrandId = request.BrandId;
                product.Description = request.Description;
                product.Capacity = request.Capacity;
                product.Size = request.Size;
                product.Flavor = request.Flavor;
                product.NicotineStrength = request.NicotineStrength;
                product.Price = request.Price;




                if (request.Images != null && request.Images.Any(i => i.File != null))
                {
                    product.Images = new List<ProductImage>();
                    int count = 0;
                    foreach (var productFile in request.Images.Where(i => i.File != null))
                    {
                        string name = await env.SaveFile(productFile.File, cancellationToken, "product");
                        count++;
                        if (count==1)
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
                    var response = new ProductCreateCommandResponse
                    {
                        Product = product,
                    };
                    return response;
                }
                catch (Exception ex)
                {
                    var response = new ProductCreateCommandResponse
                    {
                        Product = null,
                    };

                    ctx.AddModelError("Name", "Xeta bash verdi,Biraz sonra yeniden yoxlayin");

                    return response;
                }

            l1:
                return null;
            }
        }
    }




    public class ProductCreateCommandResponse
    {
        public Product Product { get; set; }
    }
}
