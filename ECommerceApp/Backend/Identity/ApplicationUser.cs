using AspNetCore.Identity.Mongo.Model;
using System.Collections.Generic;

namespace Backend.Identity
{
    public class ApplicationUser : MongoUser
    {
        public string FullName { get; set; }
        public List<Address> AddressList { get; set; } = new();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool Is2FAEnabled { get; set; } = false;
        public string TwoFactorSecret { get; set; } = string.Empty;
    }

    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string Country { get; set; }
    }
}