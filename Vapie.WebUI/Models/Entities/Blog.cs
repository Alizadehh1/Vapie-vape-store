using Vapie.WebUI.AppCode.Infrastructure;

namespace Vapie.WebUI.Models.Entities
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public string Paragraph { get; set; }
        public string ImagePath { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
