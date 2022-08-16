using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Modules.SliderModule
{
    public class SliderSingleQuery : IRequest<Slider>
    {
        public int Id { get; set; }

        public class SliderSingleQueryHandler : IRequestHandler<SliderSingleQuery, Slider>
        {
            readonly VapieDbContext db;
            public SliderSingleQueryHandler(VapieDbContext db)
            {
                this.db = db;
            }
            public async Task<Slider> Handle(SliderSingleQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Sliders
                    .FirstOrDefaultAsync(s => s.DeletedById == null && s.Id == request.Id, cancellationToken);

                return data;
            }
        }
    }
}
