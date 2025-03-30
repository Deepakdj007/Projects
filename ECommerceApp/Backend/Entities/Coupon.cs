using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Entities
{
    public class Coupon
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Code { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime ExpiresAt { get; set; }
        public int UsageLimit { get; set; }
    }
}