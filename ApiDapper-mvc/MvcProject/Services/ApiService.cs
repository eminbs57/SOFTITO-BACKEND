using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using MvcProject.Models;

namespace MvcProject.Services
{
    public class ApiService<T> where T : class
    {
        private readonly HttpClient _httpClient;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<T>>("");
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<T>(id.ToString());
        }

        public async Task CreateAsync(T entity)
        {
            var response = await _httpClient.PostAsJsonAsync("", entity);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateAsync(int id, T entity)
        {
            var response = await _httpClient.PutAsJsonAsync(id.ToString(), entity);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(id.ToString());
            response.EnsureSuccessStatusCode();
        }
    }
}
