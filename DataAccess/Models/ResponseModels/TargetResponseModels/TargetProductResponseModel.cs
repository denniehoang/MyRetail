namespace DataAccess.Models.ResponseModels;

public class TargetProductResponseModel
{
    public Data data { get; set; }
    public Error error { get; set; }
}

public class Data
{
    public Product product { get; set; }
}

public class Product
{
    public string tcin { get; set; }
    public Item item { get; set; }
}

public class Item
{
    public Product_Description product_description { get; set; }
    public Enrichment enrichment { get; set; }
    public Product_Classification product_classification { get; set; }
    public Primary_Brand primary_brand { get; set; }
}

public class Product_Description
{
    public string title { get; set; }
    public string downstream_description { get; set; }
}

public class Enrichment
{
    public Images images { get; set; }
}

public class Images
{
    public string primary_image_url { get; set; }
}

public class Product_Classification
{
    public string product_type_name { get; set; }
    public string merchandise_type_name { get; set; }
}

public class Primary_Brand
{
    public string name { get; set; }
}

public class RootError
{
    public Error[] errors { get; set; }
}

public class Error
{
    public string message { get; set; }
}
