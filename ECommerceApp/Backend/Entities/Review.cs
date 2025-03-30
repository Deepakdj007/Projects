using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Entities
{
    public class Review
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProductId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}