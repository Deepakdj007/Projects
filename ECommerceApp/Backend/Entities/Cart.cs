using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Entities
{
    public class Cart
    {
        [BsonId, BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public List<CartItem> Items { get; set; }
    }

    public class CartItem
    {
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}