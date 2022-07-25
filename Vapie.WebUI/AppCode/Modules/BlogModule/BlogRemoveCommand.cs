using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Infrastructure;
using Vapie.WebUI.Models.DataContexts;

namespace Vapie.WebUI.AppCode.Modules.BlogModule
{
    public class BlogRemoveCommand : IRequest<CommandJsonResponse>
    {
        public int Id { get; set; }
        public class BlogRemoveCommandHandler : IRequestHandler<BlogRemoveCommand, CommandJsonResponse>
        {
            readonly VapieDbContext db;
            public BlogRemoveCommandHandler(VapieDbContext db)
            {
                this.db = db;
            }
            public async Task<CommandJsonResponse> Handle(BlogRemoveCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Blogs
                    .FirstOrDefaultAsync(b => b.DeletedById == null && b.Id == request.Id, cancellationToken);

                if (entity==null)
                {
                    return new CommandJsonResponse(true, "Qeyd Movcud Deyil!");
                }

                entity.DeletedById = 1; //helelik
                entity.DeletedDate = DateTime.UtcNow.AddHours(4);
                await db.SaveChangesAsync(cancellationToken);
                return new CommandJsonResponse(false, "Deleted Successfully");
            }
        }
    }
}
