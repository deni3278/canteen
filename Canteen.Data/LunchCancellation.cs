using System;
using System.Collections.Generic;

namespace Canteen.DataAccess
{
    public partial class LunchCancellation
    {
        public LunchCancellation()
        {
            LunchMenuFridayLunchCancellations = new HashSet<LunchMenu>();
            LunchMenuMondayLunchCancellations = new HashSet<LunchMenu>();
            LunchMenuThursdayLunchCancellations = new HashSet<LunchMenu>();
            LunchMenuTuesdayLunchCancellations = new HashSet<LunchMenu>();
            LunchMenuWednesdayLunchCancellations = new HashSet<LunchMenu>();
        }

        public int LunchCancellationId { get; set; }
        public string? Message { get; set; }

        public virtual ICollection<LunchMenu> LunchMenuFridayLunchCancellations { get; set; }
        public virtual ICollection<LunchMenu> LunchMenuMondayLunchCancellations { get; set; }
        public virtual ICollection<LunchMenu> LunchMenuThursdayLunchCancellations { get; set; }
        public virtual ICollection<LunchMenu> LunchMenuTuesdayLunchCancellations { get; set; }
        public virtual ICollection<LunchMenu> LunchMenuWednesdayLunchCancellations { get; set; }
    }
}
