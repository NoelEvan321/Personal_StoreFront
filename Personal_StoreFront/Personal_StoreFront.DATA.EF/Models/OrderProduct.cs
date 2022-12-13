using System;
using System.Collections.Generic;

namespace Personal_StoreFront.DATA.EF.Models
{
    public partial class OrderProduct
    {
        public int OrderProductId { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public short? UnitQuantity { get; set; }
        public decimal? ProductPrice { get; set; }

        public virtual OrderDetail Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
