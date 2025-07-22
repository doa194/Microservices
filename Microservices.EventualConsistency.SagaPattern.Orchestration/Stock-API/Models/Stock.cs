using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Stock_API.Models
{
    public class Stock
    {
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonId]
        [BsonElement(Order = 0)]
        public ObjectId ObjectId { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        [BsonElement(Order = 1)]
        public int ProductId { get; set; }
        [BsonRepresentation(BsonType.Int32)]
        [BsonElement(Order = 2)]
        public int Count { get; set; }

        // This class represents a stock item in the inventory.

        // It contains properties for the stock's unique identifier, product ID, and count.

        // The ObjectId is used as the primary key in MongoDB, while ProductId and Count are used to track the specific product and its quantity in stock.

        // The BsonRepresentation attribute specifies how the property should be represented in MongoDB.
        // The BsonId attribute indicates that this property is the unique identifier for the document in the collection.
        // The BsonElement attribute specifies the name of the field in the MongoDB document and its order in serialization.
    }
}
