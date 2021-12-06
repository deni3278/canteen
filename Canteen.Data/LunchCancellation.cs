using System;
using System.Collections.Generic;

namespace Canteen.DataAccess
{
    public partial class LunchCancellation
    {
        public int LunchCancellationId { get; set; }
        public int LunchMenuId { get; set; }
        public string? Message { get; set; }

        public virtual LunchMenu LunchMenu { get; set; } = null!;
    }
}
