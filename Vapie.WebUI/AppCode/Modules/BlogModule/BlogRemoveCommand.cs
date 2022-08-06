using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Infrastructure;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities.Membership;
using System.Security.Claims;

namespace Vapie.WebUI.AppCode.Modules.BlogModule
{
    public class BlogRemoveCommand : IRequest<CommandJsonResponse>
    {
        public int Id { get; set; }
        public class BlogRemoveCommandHandler : IRequestHandler<BlogRemoveCommand, CommandJsonResponse>
        {
            readonly VapieDbContext db;
            readonly UserManager<VapieUser> userManager;

            public BlogRemoveCommandHandler(VapieDbContext db,UserManager<VapieUser> userManager)
            {
                this.db = db;
                this.userManager = userManager;
            }
            public async Task<CommandJsonResponse> Handle(BlogRemoveCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Blogs
                    .FirstOrDefaultAsync(b => b.DeletedById == null && b.Id == request.Id, cancellationToken);

                if (entity==null)
                {
                    return new CommandJsonResponse(true, "Qeyd Movcud Deyil!");
                }
                //var user = await userManager.GetUserAsync(User);
                entity.DeletedById = 1;
                entity.DeletedDate = DateTime.UtcNow.AddHours(4);
                await db.SaveChangesAsync(cancellationToken);
                return new CommandJsonResponse(false, "Deleted Successfully");
            }
        }
    }
}
