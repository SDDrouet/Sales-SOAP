﻿using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Entities;
using proxy.Services;
using Newtonsoft.Json;

namespace proxy
{
    public class Proxy : IService
    {
        private readonly HttpClient _httpClient;
        private const string BaseAddress = "http://localhost:5123/api";

        public Proxy(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        private async Task<T> SendPost<T, PostData>(string requestURI, PostData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            T result = default(T);
            try
            {
                requestURI = BaseAddress + requestURI;
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var jsonData = JsonConvert.SerializeObject(data);
                HttpResponseMessage response = await _httpClient.PostAsync(requestURI, new StringContent(jsonData, Encoding.UTF8, "application/json"));

                if (response.IsSuccessStatusCode)
                {
                    var resultWebAPI = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<T>(resultWebAPI);
                }
                else
                {
                    throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        private async Task<T> SendGet<T>(string requestURI)
        {
            T result = default(T);
            try
            {
                requestURI = BaseAddress + requestURI;
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var resultJSON = await _httpClient.GetStringAsync(requestURI);
                result = JsonConvert.DeserializeObject<T>(resultJSON);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        // Métodos para Categories
        public async Task<Categories> CreateCategory(Categories category)
        {
            return await SendPost<Categories, Categories>("/Category/CreateCategory", category);
        }

        public async Task<Categories> UpdateCategory(Categories category)
        {
            return await SendPost<Categories, Categories>("/Category/UpdateCategory", category);
        }

        public async Task<List<Categories>> GetAllCategories()
        {
            return await SendGet<List<Categories>>("/Category/GetAllCategories");
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var result = await SendPost<bool, int>("/Category/DeleteCategory", id);
            return result;
        }

        // Métodos para Products
        public async Task<Products> CreateProduct(Products product)
        {
            return await SendPost<Products, Products>("/Product/CreateProducts", product);
        }

        public async Task<Products> UpdateProduct(Products product)
        {
            return await SendPost<Products, Products>("/Product/UpdateProduct", product);
        }

        public async Task<List<Products>> GetAllProducts()
        {
            return await SendGet<List<Products>>("/Product/GetAllProducts");
        }

        public async Task<Products> GetProduct(int id)
        {
            return await SendGet<Products>($"/Product/GetByName{id}");
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var result = await SendPost<bool, int>("/Product/DeleteProduct", id);
            return result;
        }

        // Métodos para Logs
        public async Task<Logs> CreateLog(Logs log)
        {
            return await SendPost<Logs, Logs>("/User/Create", log);
        }

        public async Task<List<Logs>> GetAllLogs()
        {
            return await SendGet<List<Logs>>("/Log/GetAllLogs");
        }

        // Métodos para Users
        public async Task<Users> CreateUserAsync(Users user)
        {
            return await SendPost<Users, Users>("/User/Create", user);
        }

        public async Task<Users> UpdateUserAsync(Users user)
        {
            return await SendPost<Users, Users>("/User/Update", user);
        }

        public async Task<List<Users>> GetAllUsersAsync()
        {
            return await SendGet<List<Users>>("/User/Filter");
        }

        public async Task<string> LoginUserAsync(LoginRequest loginRequest)
        {
            return await SendPost<string, LoginRequest>("/User/Login", loginRequest);
        }

        public async Task<bool> ChangeUserStatusAsync(int userId)
        {
            var result = await SendPost<bool, int>("/User/ChangeStatus", userId);
            return result;
        }
    }
}
