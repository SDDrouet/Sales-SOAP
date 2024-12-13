using Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace proxy.Services
{
    public interface IService
    {
        Task<Categories> CreateCategory(Categories category);
        Task<Categories> GetCategory(int id);

        Task<string> Login(LoginRequest loginRequest);

        Task<Logs> CreateLog(Logs log);
        Task<Logs> GetLog(int id);

        Task<Products> CreateProduct(Products product);
        Task<Products> GetProduct(int id);

        Task<Users> CreateUserAsync(Users user);
        Task<Users> UpdateUserAsync(Users user);
        Task<List<Users>> GetAllUsersAsync();
        Task<string> LoginUserAsync(LoginRequest loginRequest);
        Task<bool> ChangeUserStatusAsync(int userId);
    }
}
