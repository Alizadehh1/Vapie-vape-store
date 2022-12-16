using System.Collections.Generic;
using Vapie.WebUI.AppCode.Infrastructure;
using Vapie.WebUI.Models.Entities.Membership;

namespace Vapie.WebUI.Models.Entities
{
    public class Order : BaseEntity
    {
        public bool isConfirmed { get; set; }
        public string VapieUserId { get; set; }
        public virtual VapieUser VapieUser { get; set; }
        public double TotalAmount { get; set; }
        public virtual List<OrderItem> OrderItems { get; set; }
    }
}
