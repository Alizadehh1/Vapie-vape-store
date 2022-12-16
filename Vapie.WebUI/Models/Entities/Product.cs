using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Vapie.WebUI.AppCode.Infrastructure;

namespace Vapie.WebUI.Models.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int BrandId { get; set; }
        public virtual Brand Brand { get; set; }
        public string Capacity { get; set; }
        public string Flavor { get; set; }
        public string NicotineStrength { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public double? OldPrice { get; set; }
        public virtual ICollection<ProductImage> Images { get; set; }
    }
}
