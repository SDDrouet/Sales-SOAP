using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace proxy.Services
{
    public interface IService
    {
        // Métodos para Categories
        Task<Categories> CreateCategory(Categories category);
        Task<bool> UpdateCategory(Categories category);
        Task<List<Categories>> GetAllCategories();
        Task<bool> DeleteCategory(int id);

        // Métodos para Products
        Task<Products> CreateProduct(Products product);
        Task<bool> UpdateProduct(Products product);
        Task<List<Products>> GetAllProducts();
        Task<bool> DeleteProduct(int id);

        // Métodos para Logs
        Task<List<Logs>> GetAllLogs();

        // Métodos para Users
        Task<Users> CreateUserAsync(Users user);
        Task<bool> UpdateUserAsync(Users user);
        Task<List<Users>> GetAllUsersAsync();
        Task<string> LoginUserAsync(LoginRequest loginRequest);
        Task<bool> ChangeUserStatusAsync(int userId);
    }
}
