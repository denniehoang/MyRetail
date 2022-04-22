namespace DataAccess;

public class TargetDataAccess : ITargetDataAccess
{
    public IConfiguration _config;
    public HttpClient _client;
    public string _uri;
    public string _key;

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
        return product;
    }
}
