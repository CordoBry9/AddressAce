using AddressAce.Client.Models;
using AddressAce.Client.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Json;

namespace AddressAce.Client.Services
{
    public class WASMContactDTOService : IContactDTOService
    {

        private readonly HttpClient _httpClient;

        public WASMContactDTOService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ContactDTO> CreateContactAsync(ContactDTO contact, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/contacts", contact);
            response.EnsureSuccessStatusCode();

            ContactDTO? createdDTO = await response.Content.ReadFromJsonAsync<ContactDTO>();
            return createdDTO!;
        }

        public async Task DeleteContactAsync(int contactId, string userId)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"api/contacts/{contactId}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> EmailContactAsync(int contactId, EmailData emailData, string userId)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/contacts/{contactId}/email", emailData);
            return response.IsSuccessStatusCode;
        }

        public async Task<ContactDTO?> GetContactByIdAsync(int contactId, string userId)
        {
            return await _httpClient.GetFromJsonAsync<ContactDTO>($"api/contacts/{contactId}");
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId)
        {
            return (await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>("api/contacts"))!;
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsByCategoryIdAsync(int categoryId, string userId)
        {
            return (await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>($"api/contacts?categoryId={categoryId}"))!;
        }


        public async Task<IEnumerable<ContactDTO>> SearchContactsAsync(string searchTerm, string userId)
        {
            IEnumerable<ContactDTO> response = (await _httpClient.GetFromJsonAsync<IEnumerable<ContactDTO>>($"api/contacts/search?searchTerm={searchTerm}"))!;

            return response;

        }

        public async Task UpdateContactAsync(ContactDTO contactDTO, string userId)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/contacts/{contactDTO.Id}", contactDTO);
            response.EnsureSuccessStatusCode();
        }
    }
}
