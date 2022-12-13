using System;
using System.Collections.Generic;

namespace Personal_StoreFront.DATA.EF.Models
{
    public partial class CardCondition
    {
        public CardCondition()
        {
            Products = new HashSet<Product>();
        }

        public int CardConditionId { get; set; }
        public string Condition { get; set; } = null!;
        public string? ConditionDescription { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
