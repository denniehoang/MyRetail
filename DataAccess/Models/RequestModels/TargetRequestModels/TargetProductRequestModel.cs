namespace DataAccess.Models;

public record TargetProductRequestModel(HttpClient client, int id)
{
    public string Request = $"{client.BaseAddress}&tcin={id}";
}
