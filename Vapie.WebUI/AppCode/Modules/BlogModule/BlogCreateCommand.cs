using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Extensions;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Modules.BlogModule
{
    public class BlogCreateCommand : IRequest<Blog>
    {
        public string Title { get; set; }
        public string Paragraph { get; set; }
        public string ImagePath { get; set; }
        public int? CategoryId { get; set; }
        public IFormFile File { get; set; }
        //public int[] TagIds { get; set; }
        public class BlogCreateCommandHandler : IRequestHandler<BlogCreateCommand, Blog>
        {

            readonly VapieDbContext db;
            readonly IWebHostEnvironment env;
            readonly IActionContextAccessor ctx;
            public BlogCreateCommandHandler(VapieDbContext db, IWebHostEnvironment env,IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<Blog> Handle(BlogCreateCommand request, CancellationToken cancellationToken)
            {
                if (request?.File == null)
                {
                    ctx.AddModelError("ImagePath", "Image Cannot be empty");
                }

                if (ctx.ModelIsValid())
                {
                    string fileExtension = Path.GetExtension(request.File.FileName);
                    string name = $"blog-{Guid.NewGuid()}{fileExtension}";
                    string physicalPath = Path.Combine(env.ContentRootPath, "wwwroot", "uploads", "images", name);
                    using (var fs = new FileStream(physicalPath, FileMode.Create, FileAccess.Write))
                    {
                        await request.File.CopyToAsync(fs,cancellationToken);
                    }
                    var blog = new Blog
                    {
                        Title = request.Title,
                        Paragraph = request.Paragraph,
                        CategoryId = request.CategoryId,
                        ImagePath = name
                    };
                    db.Add(blog);
                    await db.SaveChangesAsync(cancellationToken);
                   

                    return blog;
                }

                return null;
            }
        }
    }
}
