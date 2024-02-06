using kolisale.Models;
namespace kolisale.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(string? usernsme);
        Task RegisterUser(User userObj);
        Task UpdateUserInfo(int id, User userObj);
        Task DeleteUser(int id);
    }
}