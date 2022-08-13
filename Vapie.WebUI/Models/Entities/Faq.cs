using Vapie.WebUI.AppCode.Infrastructure;

namespace Vapie.WebUI.Models.Entities
{
    public class Faq : BaseEntity
    {
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
