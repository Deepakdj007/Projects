using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Entities
{
    public class SupportTicket
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Status { get; set; } = "Open";
        public string AdminUserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<TicketReply> Replies { get; set; }
    }

    public class TicketReply
    {
        public string AdminUserId { get; set; }
        public string Message { get; set; }
        public DateTime RepliedAt { get; set; } = DateTime.UtcNow;
    }
}