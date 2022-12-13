using System;
using System.Collections.Generic;

namespace Personal_StoreFront.DATA.EF.Models
{
    public partial class UsersDetail
    {
        public UsersDetail()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? CustomerCountry { get; set; }
        public string? Zip { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
