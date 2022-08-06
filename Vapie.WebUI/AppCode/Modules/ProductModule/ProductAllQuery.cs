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
    public class ProductAllQuery : IRequest<IEnumerable<Product>>
    {
        public class ProductAllQueryHandler : IRequestHandler<ProductAllQuery, IEnumerable<Product>>
        {
            private readonly VapieDbContext db;

            public ProductAllQueryHandler(VapieDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<Product>> Handle(ProductAllQuery request, CancellationToken cancellationToken)
            {
                var entity = db.Products
                    .Include(c => c.Category)
                    .Include(c => c.Brand)
                    .Include(i => i.Images.Where(i => i.IsMain == true))
                    .Where(p => p.DeletedById == null);
                return entity;
            }
        }
    }
}
