using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Modules.ProductModule
{
    public class ProductSingleQuery : IRequest<Product>
    {
        public int Id { get; set; }
        public class ProductSingleQueryHandler : IRequestHandler<ProductSingleQuery, Product>
        {
            private readonly VapieDbContext db;

            public ProductSingleQueryHandler(VapieDbContext db)
            {
                this.db = db;
            }
            public async Task<Product> Handle(ProductSingleQuery request, CancellationToken cancellationToken)
            {
                var entity = await db.Products
                    .Include(p=>p.Category)
                    .Include(p=>p.Brand)
                    .FirstOrDefaultAsync(p => p.Id == request.Id && p.DeletedById == null, cancellationToken);
                return entity;
            }
        }
    }
}
