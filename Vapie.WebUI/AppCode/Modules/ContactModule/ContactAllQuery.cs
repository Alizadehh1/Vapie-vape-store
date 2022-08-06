using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Modules.ContactModule
{
    public class ContactAllQuery : IRequest<IEnumerable<Contact>>
    {

        public class ContactAllQueryHandler : IRequestHandler<ContactAllQuery, IEnumerable<Contact>>
        {
            readonly VapieDbContext db;
            public ContactAllQueryHandler(VapieDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<Contact>> Handle(ContactAllQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Contacts
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
