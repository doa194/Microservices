using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace Shared.Services.Abstraction
{
    public interface IMongoDBService
    {
        IMongoCollection<T> GetCollection<T>(string collectionName);
        IMongoDatabase GetDatabase(string databaseName, string connectionString);
    }
}
