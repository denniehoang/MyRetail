namespace DataAccess;

public class TargetDataAccess : ITargetDataAccess
{
    private HttpClient _client;

    public TargetDataAccess(IHttpClientFactory client)
    {
        _client = client.CreateClient("Target");
    }

    public async Task<TargetProductResponseModel> GetProductById(int id)
    {
        var productRequest = new TargetProductRequestModel(_client, id);      
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
