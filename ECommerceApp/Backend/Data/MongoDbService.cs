using MongoDB.Driver;

namespace Backend.Data
{
    public class MongoDbService
    {
        private readonly IConfiguration configuration;
        private readonly IMongoDatabase? database;

        public MongoDbService(IConfiguration configuration)
        {
            this.configuration = configuration;

            var connectionString = this.configuration.GetConnectionString("DbConnection");
            var databaseName = this.configuration["MongoDB:DatabaseName"];

            if (string.IsNullOrEmpty(databaseName))
                throw new ArgumentNullException("MongoDB:DatabaseName is missing from appsettings.json");

            var mongoClient = new MongoClient(connectionString);
            this.database = mongoClient.GetDatabase(databaseName);
        }

        public IMongoDatabase? Database => this.database;
    }
}
