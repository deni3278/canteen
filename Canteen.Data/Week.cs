// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using System;
using System.Collections.Generic;

namespace Canteen.DataAccess
{
    public partial class Week
    {
        public Week()
        {
            EmployeeCakes = new HashSet<EmployeeCake>();
            LunchMenus = new HashSet<LunchMenu>();
            Orders = new HashSet<Order>();
        }

        public short Number { get; set; }
        public short Year { get; set; }
        public bool Finalized { get; set; }

        public virtual ICollection<EmployeeCake> EmployeeCakes { get; set; }
        public virtual ICollection<LunchMenu> LunchMenus { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}