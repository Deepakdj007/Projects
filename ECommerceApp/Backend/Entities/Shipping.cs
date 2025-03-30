using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Entities
{
    public class Shipping
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string OrderId { get; set; }
        public string Courier { get; set; }
        public string TrackingId { get; set; }
        public string Status { get; set; }
        public DateTime EstimatedDelivery { get; set; }
    }
}