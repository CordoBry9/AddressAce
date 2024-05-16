using AddressAce.Client.Models;
using AddressAce.Client.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace AddressAce.Client.Services
{
    public class WASMCategoryDTOService : ICategoryDTOService
    {
        private readonly HttpClient _httpClient;

        public WASMCategoryDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/categories", category);
            response.EnsureSuccessStatusCode();

            CategoryDTO? createdDTO = await response.Content.ReadFromJsonAsync<CategoryDTO>();
            return createdDTO!;
        }

        public async Task DeleteCategoryAsync(int categoryId, string userId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/categories/{categoryId}");
            response.EnsureSuccessStatusCode();
        }   

        public async Task<bool> EmailCategoryAsync(int categoryId, EmailData emailData, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/categories/{categoryId}/email", emailData);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId)
        {
            return (await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDTO>>("api/categories"))!;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId, string userId)
        {
            return (await _httpClient.GetFromJsonAsync<CategoryDTO>("api/categories"))!;
        }

        public async Task UpdateCategoryAsync(int categoryId, CategoryDTO categoryDTO, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/categories/{categoryId}", categoryDTO);
            response.EnsureSuccessStatusCode();
        }
    }
}
