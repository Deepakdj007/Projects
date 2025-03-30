using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Entities
{
    public class InventoryLog
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string AdminUserId { get; set; }
        public int OldStock { get; set; }
        public int NewStock { get; set; }
        public string Reason { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}