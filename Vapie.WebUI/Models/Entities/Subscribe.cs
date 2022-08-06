using System;
using Vapie.WebUI.AppCode.Infrastructure;

namespace Vapie.WebUI.Models.Entities
{
    public class Subscribe : BaseEntity
    {
        public string Email { get; set; }
        public bool EmailSended { get; set; } = false;
        public DateTime? AppliedDate { get; set; }
    }
}
