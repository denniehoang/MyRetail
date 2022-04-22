public class Product
{
    public string Id { get; set; }
    public string Name { get; set; }

    [JsonPropertyName("current_price")]
    public Price CurrentPrice { get; set; }
}

public class Price
{
    private decimal _value;
    private string _currencyCode;

    public Price(decimal currentPrice, string currencyCode)
    {
        _value = currentPrice;
        _currencyCode = currencyCode;
    }

    public decimal Value => _value;

    [JsonPropertyName("currency_code")]
    public string CurrencyCode => _currencyCode;
}
