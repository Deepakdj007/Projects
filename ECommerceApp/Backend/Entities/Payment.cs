using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Entities
{
    public class Payment
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public decimal AmountPaid { get; set; }
        public string Status { get; set; }
        public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    }
}