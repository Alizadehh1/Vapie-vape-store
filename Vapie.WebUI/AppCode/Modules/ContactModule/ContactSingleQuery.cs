using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Modules.ContactModule
{
    public class ContactSingleQuery : IRequest<Contact>
    {
        public int Id { get; set; }
        public class ContactSingleQueryHandler : IRequestHandler<ContactSingleQuery, Contact>
        {
            readonly VapieDbContext db;
            public ContactSingleQueryHandler(VapieDbContext db)
            {
                this.db = db;
            }
            public async Task<Contact> Handle(ContactSingleQuery request, CancellationToken cancellationToken)
            {

                var model = await db.Contacts
                    .FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken);

                return model;
            }
        }
    }
}
