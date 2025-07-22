using MongoDB.Driver;

namespace Stock_API.Context
{
    public class MongoDBContext
    {

        // This field holds the MongoDB database instance.
        // It is marked as readonly to ensure it is initialized only once and cannot be changed later.
        
        readonly IMongoDatabase _database;
        
        // This constructor initializes the MongoDB context using the connection string from the configuration.
        // It creates a MongoClient and retrieves the database named "StockDB".
        
        public MongoDBContext(IConfiguration configuration)
        {
            MongoClient client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            _database = client.GetDatabase("StockDB");
        }

        // Generic method to get a collection of type T
        // This method retrieves a collection from the MongoDB database. It uses the name of the type T as the collection name, converted to lowercase. This allows for flexibility in working with different types of collections.
        
        public IMongoCollection<T> GetCollection<T>()
        {
            return _database.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }
    }
}
