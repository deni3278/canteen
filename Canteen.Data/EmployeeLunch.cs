using System;
using System.Collections.Generic;

namespace Canteen.DataAccess
{
    public partial class EmployeeLunch
    {
        public int EmployeeId { get; set; }
        public int LunchMenuId { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }

        public virtual Employee Employee { get; set; } = null!;
        public virtual LunchMenu LunchMenu { get; set; } = null!;
    }
}
