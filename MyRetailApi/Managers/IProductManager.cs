namespace MyRetailApi.Managers
{
    public interface IProductManager
    {
        Task<TargetProductResponseModel> GetTargetProductById(int id);
        Task<DbProductResponseModel> GetDBProductById(int id);
        Product GetAggregateProduct(TargetProductResponseModel targetModel, DbProductResponseModel dbModel);
        //Task InsertProductPrice(int id, decimal price, CurrencyCode currency);
        Task UpdateProductPrice(DbProductResponseModel product);
    }

    public class ProductManager : IProductManager
    {
        private readonly ITargetDataAccess _targetDataAccess;
        private readonly IProductDB _productDB;

        public ProductManager(ITargetDataAccess targetDataAccess, IProductDB productDB)
        {
            _targetDataAccess = targetDataAccess;
            _productDB = productDB;
        }

        public async Task<TargetProductResponseModel> GetTargetProductById(int id)
        {
            return await _targetDataAccess.GetProductById(id);
        }

        public async Task<DbProductResponseModel> GetDBProductById(int id)
        {
            return await _productDB.GetProductPriceById(id);
        }

        public Product GetAggregateProduct(TargetProductResponseModel targetModel, DbProductResponseModel dbModel)
        {
            var price = Convert.ToDecimal(dbModel.Price);
            var currencyCode = dbModel.Currency;

            var product = new Product()
            {
                Id = targetModel.data.product.tcin,
                Name = targetModel.data.product.item.product_description.title,
                CurrentPrice = new Price(price, currencyCode.ToString())
            };

            //TODO: possible caching depending on aggregate product
            return product;
        }

        //public async Task InsertProductPrice(int id, decimal price, CurrencyCode currency)
        //{
        //    await _productDB.Insert(id.ToString(), price, currency);
        //}

        public async Task UpdateProductPrice(DbProductResponseModel product)
        {
            await _productDB.UpdateProduct(product);
        }
    }
}
