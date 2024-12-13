using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;
using SLC;

namespace proxy.Services
{
    internal interface IService
    {
        // Métodos para Categories
        Task<Categories> CreateCategory(Categories category);
        Task<Categories> GetCategory(int id);

        // Métodos para LoginRequest
        Task<string> Login(LoginRequest loginRequest);

        // Métodos para Logs
        Task<Logs> CreateLog(Logs log);
        Task<Logs> GetLog(int id);

        // Métodos para Products
        Task<Products> CreateProduct(Products product);
        Task<Products> GetProduct(int id);

        // Métodos para Users
        Task<Users> CreateUser(Users user);
        Task<Users> GetUser(int id);
    }
}
