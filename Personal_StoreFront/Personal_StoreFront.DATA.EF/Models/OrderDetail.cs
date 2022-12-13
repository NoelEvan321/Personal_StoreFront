using System;
using System.Collections.Generic;

namespace Personal_StoreFront.DATA.EF.Models
{
    public partial class OrderDetail
    {
        public OrderDetail()
        {
            OrderProducts = new HashSet<OrderProduct>();
        }

        public int OrderId { get; set; }
        public string UserId { get; set; } = null!;
        public DateTime OrderDate { get; set; }
        public string ShipToName { get; set; } = null!;
        public string ShipCity { get; set; } = null!;
        public string? ShipState { get; set; }
        public string? ShipZip { get; set; }
        public string ShipCountry { get; set; } = null!;
        public bool? IsActive { get; set; }

        public virtual UsersDetail User { get; set; } = null!;
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
