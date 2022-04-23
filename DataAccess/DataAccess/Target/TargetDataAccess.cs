namespace DataAccess;

public class TargetDataAccess : ITargetDataAccess
{
    private IConfiguration _config;
    private HttpClient _client;
    private string _uri;
    private string _key;

    public TargetDataAccess(IConfiguration config)
    {
        _config = config;
        _client = new HttpClient();
        _uri = config["target:uri"];
        _key = config["target:key"];
    }

    public async Task<TargetProductResponseModel> GetProductById(int id)
    {
        var productRequest = new TargetProductRequestModel(_uri, _key, id);
        var response = await _client.GetAsync(productRequest.Request.ToString());
        var result = response.Content.ReadAsStringAsync().Result;
        var product = JsonSerializer.Deserialize<TargetProductResponseModel>(result);
        if (product.data is null)
        {
            var errorObj = JsonSerializer.Deserialize<RootError>(result);
            throw new Exception(String.Format("{0}", errorObj.errors.First().message));
        }

        return product;
    }
}
