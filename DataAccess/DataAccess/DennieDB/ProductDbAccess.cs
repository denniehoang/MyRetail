namespace DataAccess.DataAccess.DennieDB;

public class ProductDbAccess : IProductDB
{
    private MongoClient _client;
    private IMongoDatabase _database;
    private IMongoCollection<DbProductResponseModel> _collection;

    public ProductDbAccess(IConfiguration config)
    {
        _client = new MongoClient(config["dennieDB:connectionString"]);
        _database = _client.GetDatabase(config["dennieDB:databaseName"]);
        _collection = _database.GetCollection<DbProductResponseModel>(config["dennieDB:collectionName"]);
    }

    public async Task<DbProductResponseModel> GetProductPriceById(int id)
    {
        var result = await _collection.FindAsync(x => x.Id.Equals(id.ToString()));
        return result.FirstOrDefault();
    }

    public async Task Insert(string id, decimal price, CurrencyCode currency)
    {
        await _collection.InsertOneAsync(new DbProductResponseModel { Id = id, Price = price, Currency = currency });
    }

    public async Task UpdateProduct(DbProductResponseModel product)
    {
        var filter = Builders<DbProductResponseModel>.Filter.Eq("Id", product.Id);
        await _collection.ReplaceOneAsync(filter, product, new ReplaceOptions { IsUpsert = true }) ;      
    }
}
