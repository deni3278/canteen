using System;
using System.Collections.Generic;

namespace Canteen.DataAccess
{
    public partial class Item
    {
        public Item()
        {
            EmployeeCakes = new HashSet<EmployeeCake>();
            LunchMenuFridayItems = new HashSet<LunchMenu>();
            LunchMenuMondayItems = new HashSet<LunchMenu>();
            LunchMenuThursdayItems = new HashSet<LunchMenu>();
            LunchMenuTuesdayItems = new HashSet<LunchMenu>();
            LunchMenuWednesdayItems = new HashSet<LunchMenu>();
            OrderItems = new HashSet<OrderItem>();
            Employees = new HashSet<Employee>();
        }

        public int ItemId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public byte[] Image { get; set; } = null!;
        
        public bool Active { get; set; }

        public virtual Category Category { get; set; } = null!;
        public virtual ICollection<EmployeeCake> EmployeeCakes { get; set; }
        public virtual ICollection<LunchMenu> LunchMenuFridayItems { get; set; }
        public virtual ICollection<LunchMenu> LunchMenuMondayItems { get; set; }
        public virtual ICollection<LunchMenu> LunchMenuThursdayItems { get; set; }
        public virtual ICollection<LunchMenu> LunchMenuTuesdayItems { get; set; }
        public virtual ICollection<LunchMenu> LunchMenuWednesdayItems { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
