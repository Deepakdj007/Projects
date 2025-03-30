using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Entities
{
    public class AdminLog
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string AdminUserId { get; set; }
        public string Action { get; set; }
        public string EntityAffected { get; set; }
        public DateTime ActionTime { get; set; } = DateTime.UtcNow;
        public string IPAddress { get; set; }
    }
}