using System.Text.Json;

namespace MyRetailApi.Controllers;

[ApiController]
public class MyRetailController : ControllerBase
{
    private readonly ILogger<MyRetailController> _logger;
    private IProductManager _deviceManager;

    //TODO: Could add cancellations to each request, if aggregate product can be large.
    public MyRetailController(ILogger<MyRetailController> logger, IProductManager deviceManager)
    {
        _logger = logger;
        _deviceManager = deviceManager;
    }

    [HttpGet("products/{id}")]
    public async Task<IActionResult> GetProductById(int id = 13860428)
    {
        try
        {
            var targetObj = await _deviceManager.GetTargetProductById(id);
            var dbObj = await _deviceManager.GetDBProductById(id);

            return Ok(_deviceManager.GetAggregateProduct(targetObj, dbObj));
        }
        catch (Exception ex)
        {
            _logger.LogError("{0}\n{1}", ex.Message, ex.StackTrace);
            return NotFound(ex.Message);
        }
    }

    [HttpPost("products")]
    public async Task InsertProductPrice(int id, decimal price, CurrencyCode currency)
    {
        await _deviceManager.InsertProductPrice(id, price, currency);
    }

    [HttpPut("products/{id}")]
    public async Task<IActionResult> UpdateProductPrice(DbProductResponseModel product, int id = 13860428)
    {
        try
        {
            if (!product.Id.Equals(id.ToString()))
                throw new Exception(String.Format("{0} Id does not match {1}", JsonSerializer.Serialize(product), id));
            await _deviceManager.UpdateProductPrice(product);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError("{0}\n{1}", ex.Message, ex.StackTrace);
            return BadRequest(ex.Message);
        }
    }

}
