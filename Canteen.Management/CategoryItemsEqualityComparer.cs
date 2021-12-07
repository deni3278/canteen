using System;
using System.Collections.Generic;
using System.Linq;
using Canteen.Dto;

namespace Canteen.Management;

public class CategoryItemsEqualityComparer : IEqualityComparer<CategoryItemsDto>
{
    public bool Equals(CategoryItemsDto x, CategoryItemsDto y)
    {
        if (ReferenceEquals(x, y))
            return true;
        
        if (ReferenceEquals(x, null))
            return false;
        
        if (ReferenceEquals(y, null))
            return false;
        
        if (x.GetType() != y.GetType())
            return false;
        
        return x.CategoryId == y.CategoryId &&
               x.Name == y.Name &&
               x.Items.All(y.Items.Contains) &&
               x.Items.Count == y.Items.Count;
    }

    public int GetHashCode(CategoryItemsDto obj)
    {
        return HashCode.Combine(obj.CategoryId, obj.Name);
    }
}