using System;
using System.Collections.Generic;

namespace Personal_StoreFront.DATA.EF.Models
{
    public partial class Product
    {
        public Product()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string? ProductName { get; set; } = null!;
        public int? CardConditionId { get; set; }
        public string? ProductSeries { get; set; }
        public string? CardDescription { get; set; }
        public string? ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public string? ProductPicture { get; set; }
        public int? TypeId { get; set; }

        public virtual CardCondition? CardCondition { get; set; }
        public virtual Category? Category { get; set; }
        public virtual ProductType? Type { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
