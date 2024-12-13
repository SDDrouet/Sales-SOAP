using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Entities; // Asegúrate de que esta referencia es correcta
using NWindProxyService.Services;

namespace NWindProxyService
{
    public class Proxy : IService
    {
        private const string BaseAddress = "http://localhost:5123/api";

        private async Task<T> SendPost<T, PostData>(string requestURI, PostData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            T result = default(T);
            using (var client = new HttpClient())
            {
                try
                {
                    requestURI = BaseAddress + requestURI;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var jsonData = JsonConvert.SerializeObject(data);
                    HttpResponseMessage response = await client.PostAsync(requestURI, new StringContent(jsonData, Encoding.UTF8, "application/json"));

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
            }
            return result;
        }

        private async Task<T> SendGet<T>(string requestURI)
        {
            T result = default(T);
            using (var client = new HttpClient())
            {
                try
                {
                    requestURI = BaseAddress + requestURI;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var resultJSON = await client.GetStringAsync(requestURI);
                    result = JsonConvert.DeserializeObject<T>(resultJSON);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
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
        public async Task<LoginRequest> Login(LoginRequest loginRequest)
        {
            return await SendPost<LoginRequest, LoginRequest>("/login", loginRequest);
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

        // Métodos para Users
        public async Task<Users> CreateUser(Users user)
        {
            return await SendPost<Users, Users>("/users", user);
        }

        public async Task<Users> GetUser(int id)
        {
            return await SendGet<Users>($"/users/{id}");
        }
    }
}
