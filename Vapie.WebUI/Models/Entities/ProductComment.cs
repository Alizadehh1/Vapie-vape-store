using Vapie.WebUI.AppCode.Infrastructure;

namespace Vapie.WebUI.Models.Entities
{
    public class ProductComment : BaseEntity
    {
        public string Comment { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string UserName { get; set; }
    }
}
