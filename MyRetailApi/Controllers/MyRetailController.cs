using DataAccess.Models.ResponseModels.DbResponseModels;

namespace MyRetailApi.Controllers;

[ApiController]
public class MyRetailController : ControllerBase
{
    public readonly ITargetDataAccess _targetDataAccess;
    public readonly IProductDB _productDB;

    public MyRetailController(ITargetDataAccess targetDataAccess, IProductDB productDB)
    {
        _targetDataAccess = targetDataAccess;
        _productDB = productDB;
    }

    [HttpGet("products/{id}")]
    public async Task<Product> GetProductById(int id = 13860428)
    {
        var targetObj = await _targetDataAccess.GetProductById(id);
        var dbObj = await _productDB.GetProductPriceById(id);

        var price = Convert.ToDecimal(dbObj.Price);
        var currencyCode = dbObj.Currency;
            //(CurrencyCode)Enum.Parse(typeof(CurrencyCode), dbObj.Currency);

        var product = new Product()
        {
            Id = targetObj.data.product.tcin,
            Name = targetObj.data.product.item.product_description.title,
            CurrentPrice = new Price(price, currencyCode.ToString())
        };

        return product;
    }

    [HttpPost("products")]
    public async Task InsertProductPrice(int id, decimal price, CurrencyCode currency)
    {
        await _productDB.Insert(id.ToString(), price, currency);
    }

    [HttpPut("products/{id}")]
    public async Task UpdateProductPrice(DbProductResponseModel product, int id = 13860428)
    {
        await _productDB.UpdateProduct(product);
    }
}
