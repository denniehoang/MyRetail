namespace DataAccess.Models.ResponseModels.DbResponseModels;

public class DbProductResponseModel
{
    [BsonId]
    public string Id { get; set; }
    public decimal Price { get; set; }
    public CurrencyCode Currency { get; set; }
}

public enum CurrencyCode
{
    USD
}