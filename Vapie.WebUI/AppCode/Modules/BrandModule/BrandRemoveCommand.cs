using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Infrastructure;
using Vapie.WebUI.Models.DataContexts;

namespace Vapie.WebUI.AppCode.Modules.BrandModule
{
    public class BrandRemoveCommand : IJsonRequest
    {
        public int Id { get; set; }

        public class BrandRemoveCommandHandler : IJsonRequestHandler<BrandRemoveCommand>
        {
            readonly VapieDbContext db;
            public BrandRemoveCommandHandler(VapieDbContext db)
            {
                this.db = db;
            }
            public async Task<CommandJsonResponse> Handle(BrandRemoveCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Brands
                       .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                if (entity == null)
                {
                    return new CommandJsonResponse(true,"Qeyd movcud deyil!");
                }

                entity.DeletedById = 1;
                entity.DeletedDate = DateTime.UtcNow.AddHours(4);
                await db.SaveChangesAsync(cancellationToken);

                return new CommandJsonResponse(false, "Qeyd silindi!");
            }
        }
    }
}
