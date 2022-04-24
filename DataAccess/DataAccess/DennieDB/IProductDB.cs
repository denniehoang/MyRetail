namespace DataAccess.DataAccess.DennieDB;

public interface IProductDB
{
    public Task<DbProductResponseModel> GetProductPriceById(int id);
    //public Task Insert(string id, decimal price, CurrencyCode currency);
    public Task UpdateProduct(DbProductResponseModel product);
}
