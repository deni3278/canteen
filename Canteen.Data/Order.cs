using System;
using System.Collections.Generic;

namespace Canteen.DataAccess
{
    public partial class Order
    {
        public Order()
        {
            OrderItems = new HashSet<OrderItem>();
        }

        public int OrderId { get; set; }
        public int EmployeeId { get; set; }
        public short Number { get; set; }
        public short Year { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual Week Week { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
