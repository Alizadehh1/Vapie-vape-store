using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Modules.SliderModule
{
    public class SliderAllQuery : IRequest<IEnumerable<Slider>>
    {
        public class SliderAllQueryHandler : IRequestHandler<SliderAllQuery, IEnumerable<Slider>>
        {
            readonly VapieDbContext db;
            public SliderAllQueryHandler(VapieDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<Slider>> Handle(SliderAllQuery request, CancellationToken cancellationToken)
            {
                var data = db.Sliders
                    .Where(s => s.DeletedById == null);

                return data;
            }
        }
    }
}
