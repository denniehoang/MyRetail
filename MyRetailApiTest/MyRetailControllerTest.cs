using System.Threading.Tasks;
using DataAccess;
using DataAccess.DataAccess.DennieDB;
using DataAccess.Models.ResponseModels;
using DataAccess.Models.ResponseModels.DbResponseModels;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyRetailApi.Managers;

namespace MyRetailApiTest;

[TestClass]
public class MyRetailControllerTest
{
    private Mock<ITargetDataAccess> _mockTargetAccess;
    private Mock<IProductDB> _mockProductDb;

    public MyRetailControllerTest()
    {
        _mockTargetAccess = new Mock<ITargetDataAccess>();
        _mockProductDb = new Mock<IProductDB>();
    }

    [TestInitialize]
    public void Setup()
    {
        var targetResponse = new TargetProductResponseModel { data = new Data() };
        targetResponse.data.product = new DataAccess.Models.ResponseModels.Product()
        {
            tcin = "13860428",
            item = new Item()
            {
                product_description = new Product_Description()
                {
                    title = "The Big Lebowski(Blu - ray)(Widescreen)"
                }
            }
        };

        var dbResponse = new DbProductResponseModel()
        {
            Id = "13860428",
            Price = 13.49m,
            Currency = CurrencyCode.USD,
        };

        _mockProductDb.Setup(x => x.GetProductPriceById(13860428)).Returns(Task.FromResult(dbResponse));
        _mockTargetAccess.Setup(x => x.GetProductById(13860428)).Returns(Task.FromResult(targetResponse));
    }

    [TestMethod]
    public void TestGetProductById()
    {
        var manager = new ProductManager(_mockTargetAccess.Object, _mockProductDb.Object);
        var targetObj = manager.GetTargetProductById(13860428).Result;
        var dbObj = manager.GetDBProductById(13860428).Result;
        var actual = manager.GetAggregateProduct(targetObj, dbObj);
        var expected = new Product
        {
            Id = "13860428",
            Name = "The Big Lebowski(Blu - ray)(Widescreen)",
            CurrentPrice = new Price(13.49m, "USD") { }
        };

        actual.Should().BeEquivalentTo(expected);
    }

    [TestMethod]
    public async Task TestUpdateProductPrice()
    {
        var manager = new ProductManager(_mockTargetAccess.Object, _mockProductDb.Object);
        var dbObj = manager.GetDBProductById(13860428).Result;

        Assert.AreEqual(13.49m, dbObj.Price);

        var updatedProduct = new DbProductResponseModel()
        {
            Id = "13860428",
            Price = 15m,
            Currency = CurrencyCode.USD
        };

        _mockProductDb.Setup(x => x.UpdateProduct(dbObj)).Returns(Task.Run(() => _mockProductDb.Setup(x => x.GetProductPriceById(13860428)).Returns(Task.FromResult(updatedProduct))));

        await manager.UpdateProductPrice(dbObj);

        dbObj = manager.GetDBProductById(13860428).Result;

        Assert.AreEqual(15m, dbObj.Price);
    }
}
