using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Shared.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 0)]
        public string ProductId { get; set; }
        [BsonRepresentation(BsonType.String)]
        [BsonElement(Order = 1)]
        public string ProductName { get; set; }
        [BsonRepresentation(BsonType.Boolean)]
        [BsonElement(Order = 2)]
        public bool IsAvailable { get; set; }
        [BsonRepresentation(BsonType.Int64)]
        [BsonElement(Order = 3)]
        public decimal ProductPrice { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        [BsonElement(Order = 4)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
