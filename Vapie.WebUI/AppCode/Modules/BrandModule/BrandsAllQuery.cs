using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Modules.BrandModule
{
    public class BrandsAllQuery : IRequest<IEnumerable<Brand>>
    {

        public class BrandsAllQueryHandler : IRequestHandler<BrandsAllQuery, IEnumerable<Brand>>
        {
            readonly VapieDbContext db;
            public BrandsAllQueryHandler(VapieDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<Brand>> Handle(BrandsAllQuery request, CancellationToken cancellationToken)
            {
                var model = db.Brands
                    .Where(b => b.DeletedById == null);

                return model;
            }
        }
    }
}
