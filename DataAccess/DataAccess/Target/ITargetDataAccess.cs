namespace DataAccess;

public interface ITargetDataAccess
{
    Task<TargetProductResponseModel> GetProductById(int id);
}
