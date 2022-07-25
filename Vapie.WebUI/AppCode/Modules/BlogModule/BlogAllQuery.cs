using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Modules.BlogModule
{
    public class BlogAllQuery : IRequest<IEnumerable<Blog>>
    {
        public class BlogAllQueryHandler : IRequestHandler<BlogAllQuery, IEnumerable<Blog>>
        {
            readonly VapieDbContext db;
            public BlogAllQueryHandler(VapieDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<Blog>> Handle(BlogAllQuery request, CancellationToken cancellationToken)
            {
                var entity = db.Blogs
                 .Include(b => b.Category)
                .Where(b => b.DeletedById == null);
                return entity;
            }
        }
    }
}
