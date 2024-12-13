using System;
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
            return await SendPost<Categories, Categories>("/categories", category);
        }

        public async Task<Categories> GetCategory(int id)
        {
            return await SendGet<Categories>($"/categories/{id}");
        }

        // Métodos para LoginRequest
        public async Task<string> Login(LoginRequest loginRequest)
        {
            return await SendPost<string, LoginRequest>("/User/Login", loginRequest);
        }

        // Métodos para Logs
        public async Task<Logs> CreateLog(Logs log)
        {
            return await SendPost<Logs, Logs>("/logs", log);
        }

        public async Task<Logs> GetLog(int id)
        {
            return await SendGet<Logs>($"/logs/{id}");
        }

        // Métodos para Products
        public async Task<Products> CreateProduct(Products product)
        {
            return await SendPost<Products, Products>("/products", product);
        }

        public async Task<Products> GetProduct(int id)
        {
            return await SendGet<Products>($"/products/{id}");
        }

        // Métodos para Users (Implementación de los métodos faltantes)
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
            return await SendGet<List<Users>>("/users");
        }

        public async Task<string> LoginUserAsync(LoginRequest loginRequest)
        {
            return await SendPost<string, LoginRequest>("/User/Login", loginRequest);
        }

        public async Task<bool> ChangeUserStatusAsync(int userId)
        {
            // Suponiendo que hay un endpoint para cambiar el estado del usuario
            var result = await SendPost<bool, int>("/User/ChangeStatus", userId);
            return result;
        }
    }
}
