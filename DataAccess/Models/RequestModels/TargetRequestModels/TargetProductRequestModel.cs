namespace DataAccess.Models;

public record TargetProductRequestModel(string uri, string key, int id)
{
    public string Request = String.Format("{0}?key={1}&tcin={2}", uri, key, id);
}
