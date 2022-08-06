using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using Vapie.WebUI.AppCode.Extensions;
using Vapie.WebUI.Models.DataContexts;
using Vapie.WebUI.Models.Entities;

namespace Vapie.WebUI.AppCode.Modules.ContactModule
{
    public class ContactAnswerCommand : IRequest<Contact>
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Cannot be Empty")]
        [MinLength(3, ErrorMessage = "Cannot be less than three symbol")]
        public string AnswerMessage { get; set; }
        public class ContactAnswerCommandHandler : IRequestHandler<ContactAnswerCommand, Contact>
        {
            readonly VapieDbContext db;
            readonly IActionContextAccessor ctx;
            public ContactAnswerCommandHandler(VapieDbContext db,IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }

            public async Task<Contact> Handle(ContactAnswerCommand request, CancellationToken cancellationToken)
            {
                l1:
                if (!ctx.ModelIsValid())
                {
                    return new Contact
                    {
                        AnswerMessage = request.AnswerMessage,
                        Id = request.Id
                    };
                }

                var post = await db.Contacts
                    .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

                if (post==null)
                {
                    ctx.AddModelError("AnswerMessage", "Not Found");
                    goto l1;
                }
                else if(post.AnsweredById!=null)
                {
                    ctx.AddModelError("AnswerMessage", "Already Answered");
                }
                post.AnsweredById = 1;
                post.AnswerDate = DateTime.UtcNow.AddHours(4);
                post.AnswerMessage = request.AnswerMessage;
                await db.SaveChangesAsync(cancellationToken);
                return post;
            }
        }
    }
}
