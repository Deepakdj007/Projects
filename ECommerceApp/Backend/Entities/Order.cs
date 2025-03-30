using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Entities
{
    public class Order
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
        public Address ShippingAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string OrderStatus { get; set; } = "Placed";
        public DateTime OrderedAt { get; set; } = DateTime.UtcNow;
    }

    public class OrderItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
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