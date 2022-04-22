using System.Threading.Tasks;
using DataAccess;
using DataAccess.DataAccess.DennieDB;
using DataAccess.Models.ResponseModels;
using DataAccess.Models.ResponseModels.DbResponseModels;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyRetailApi.Controllers;

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
            Price = 13.49m,
            Currency = CurrencyCode.USD,
        };

        _mockProductDb.Setup(x => x.GetProductPriceById(It.IsAny<int>())).Returns(Task.FromResult(dbResponse));
        _mockTargetAccess.Setup(x => x.GetProductById(It.IsAny<int>())).Returns(Task.FromResult(targetResponse));
    }

    [TestMethod]
    public void TestGetProductById()
    {
        var controller = new MyRetailController(_mockTargetAccess.Object, _mockProductDb.Object);
        var actual = controller.GetProductById(It.IsAny<int>()).Result;
        var expected = new Product
        {
            Id = "13860428",
            Name = "The Big Lebowski(Blu - ray)(Widescreen)",
            CurrentPrice = new Price(13.49m, "USD") { }
        };

        actual.Should().BeEquivalentTo(expected);
    }
}
