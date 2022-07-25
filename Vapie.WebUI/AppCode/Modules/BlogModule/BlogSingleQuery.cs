using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Modules.BlogModule
{
    public class BlogSingleQuery : IRequest<Blog>
    {
        public int Id { get; set; }
        public class BlogSingleQueryHandler : IRequestHandler<BlogSingleQuery, Blog>
        {
            readonly VapieDbContext db;
            public BlogSingleQueryHandler(VapieDbContext db)
            {
                this.db = db;
            }
            public async Task<Blog> Handle(BlogSingleQuery request, CancellationToken cancellationToken)
            {

                var blog = await db.Blogs
                    .Include(b => b.Category)
                    //.Include(b=> b.TagCloud)
                    .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedById == null, cancellationToken);

                return blog;
            }
        }
    }
}
