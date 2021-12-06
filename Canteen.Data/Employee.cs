using System;
using System.Collections.Generic;

namespace Canteen.DataAccess
{
    public partial class Employee
    {
        public Employee()
        {
            EmployeeCakes = new HashSet<EmployeeCake>();
            EmployeeLunches = new HashSet<EmployeeLunch>();
            Orders = new HashSet<Order>();
            Items = new HashSet<Item>();
        }

        public int EmployeeId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Password { get; set; } = null!;

        public virtual ICollection<EmployeeCake> EmployeeCakes { get; set; }
        public virtual ICollection<EmployeeLunch> EmployeeLunches { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
