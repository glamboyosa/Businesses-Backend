using System;
using System.Collections.Generic;

namespace TheBackend.Models
{
    public partial class BusinessListing
    {
        public long Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string BusinessName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Lga { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }

        public Categories CategoryNameNav { get; set; }
    }
}
