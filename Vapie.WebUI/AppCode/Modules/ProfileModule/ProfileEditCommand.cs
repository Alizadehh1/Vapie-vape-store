using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Extensions;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities.Membership;

namespace Vapie.WebUI.AppCode.Modules.ProfileModule
{
    public class ProfileEditCommand : IRequest<VapieUser>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string ImagePath { get; set; }
        public IFormFile File { get; set; }
        public class ProfileEditCommandHandler : IRequestHandler<ProfileEditCommand, VapieUser>
        {
            readonly VapieDbContext db;
            private readonly IWebHostEnvironment env;
            private readonly IActionContextAccessor ctx;

            public ProfileEditCommandHandler(VapieDbContext db,IWebHostEnvironment env,IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<VapieUser> Handle(ProfileEditCommand request, CancellationToken cancellationToken)
            {
                if (ctx.ModelIsValid())
                {
                    if (request.File == null && string.IsNullOrEmpty(request.ImagePath))
                    {
                        ctx.AddModelError("ImagePath", "Image Cannot be empty");
                    }
                    var currentEntity = await db.Users
                    .FirstOrDefaultAsync(b => b.Id == request.Id , cancellationToken);
                    if (currentEntity == null)
                    {
                        return null;
                    }
                    string oldFileName = currentEntity.ImagePath;
                    if (request.File != null)
                    {
                        string fileExtension = Path.GetExtension(request.File.FileName);
                        string name = $"user-{Guid.NewGuid()}{fileExtension}";
                        string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", name);
                        using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                        {
                            request.File.CopyTo(fs);
                        }
                        currentEntity.ImagePath = name;
                        string physicalPathOld = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", oldFileName);
                        if (System.IO.File.Exists(physicalPathOld))
                        {
                            System.IO.File.Delete(physicalPathOld);
                        }
                    }
                    currentEntity.Name = request.Name;
                    currentEntity.Surname = request.Surname;
                    currentEntity.UserName = request.UserName;
                    await db.SaveChangesAsync(cancellationToken);
                    return currentEntity;
                }
                return null;
            }
        }
    }
}
