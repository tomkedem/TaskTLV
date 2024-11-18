using ProductAPI.Entities.ProductAPI.Entities;

public interface IUserRepository
{
    User GetUserByUsername(string username);
}
