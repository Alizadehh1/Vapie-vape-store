using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Modules.BrandModule
{
    public class BrandCreateCommand : IRequest<Brand>
    {
        public string Name { get; set; }
        public class BrandCreateCommandHandler : IRequestHandler<BrandCreateCommand, Brand>
        {
            readonly VapieDbContext db;
            public BrandCreateCommandHandler(VapieDbContext db)
            {
                this.db = db;
            }
            public async Task<Brand> Handle(BrandCreateCommand request, CancellationToken cancellationToken)
            {
                var brand = new Brand();
                brand.Name = request.Name;

                await db.Brands.AddAsync(brand);
                await db.SaveChangesAsync(cancellationToken);

                return brand;
            }
        }
    }
}
