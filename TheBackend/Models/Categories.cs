using System;
using System.Collections.Generic;

namespace TheBackend.Models
{
    public partial class Categories
    {
        public Categories()
        {
            BusinessListing = new HashSet<BusinessListing>();
        }

        public long Id { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<BusinessListing> BusinessListing { get; set; }
    }
}
