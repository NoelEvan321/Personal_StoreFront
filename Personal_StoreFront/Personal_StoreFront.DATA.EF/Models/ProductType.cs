using System;
using System.Collections.Generic;

namespace Personal_StoreFront.DATA.EF.Models
{
    public partial class ProductType
    {
        public ProductType()
        {
            Products = new HashSet<Product>();
        }

        public int TypeId { get; set; }
        public string ProductType1 { get; set; } = null!;
        public string? ProductTypeDescription { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
