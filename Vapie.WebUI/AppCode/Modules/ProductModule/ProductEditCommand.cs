using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
    public class ProductEditCommand : IRequest<ProductEditCommandResponse>
    {
        public int Id { get; set; }
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

        public class ProductEditCommandHandler : IRequestHandler<ProductEditCommand, ProductEditCommandResponse>
        {
            readonly VapieDbContext db;
            readonly IWebHostEnvironment env;
            readonly IActionContextAccessor ctx;

            public ProductEditCommandHandler(VapieDbContext db, IWebHostEnvironment env, IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<ProductEditCommandResponse> Handle(ProductEditCommand request, CancellationToken cancellationToken)
            {

                var response = new ProductEditCommandResponse
                {
                    Product = null,
                };

                if (!ctx.ModelIsValid())
                {

                    return response;
                }

                var product = await db.Products
                    .Include(p => p.Images)
                    .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
                if (product == null)
                {
                    ctx.AddModelError("Name", "Product tapilmadi");
                    return response;
                }


                product.Name = request.Name;
                product.BrandId = request.BrandId;
                product.CategoryId = request.CategoryId;
                product.Description = request.Description;
                product.Capacity = request.Capacity;
                product.Size = request.Size;
                product.NicotineStrength = request.NicotineStrength;
                product.Flavor = request.Flavor;
                product.Price = request.Price;


                if (request.Images != null)
                {
                    if (product.Images == null)
                    {
                        product.Images = new List<ProductImage>();
                    }
                    int count = 0;
                    foreach (var productFile in request.Images)
                    {
                        if (productFile.File == null && string.IsNullOrWhiteSpace(productFile.TempPath))
                        {
                            var dbProductImage = product.Images.FirstOrDefault(p => p.Id == productFile.Id);

                            if (dbProductImage != null)
                            {
                                dbProductImage.DeletedById = 1;
                                dbProductImage.DeletedDate = DateTime.UtcNow.AddHours(4);
                                dbProductImage.IsMain = false;
                            }
                        }
                        else if (productFile.File != null)
                        {
                            string name = await env.SaveFile(productFile.File, cancellationToken, "product");

                            count++;
                            if (count == 1)
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
                        else if (!string.IsNullOrWhiteSpace(productFile.TempPath))
                        {
                            var dbProductImage = product.Images.FirstOrDefault(p => p.Id == productFile.Id);

                            if (dbProductImage != null)
                            {
                                dbProductImage.IsMain = false;
                            }
                        }
                    }
                }
                else
                {
                    ctx.AddModelError("Images", "Sekil qeyd edilmeyib");
                    goto l1;
                }


                

                try
                {
                    await db.SaveChangesAsync(cancellationToken);
                    response.Product = product;
                    return response;
                }
                catch (Exception ex)
                {
                    response.Product = product;

                    ctx.AddModelError("Name", "Xeta bash verdi,Biraz sonra yeniden yoxlayin");

                    return response;
                }

            l1:
                return null;
            }
        
        }
    }
}
public class ProductEditCommandResponse
{
    public Product Product { get; set; }
}
