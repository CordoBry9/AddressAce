using AddressAce.Models;

namespace AddressAce.Services.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact> CreateContactAsync(Contact contact);
        Task AddCategoriesToContactAsync(int contactId, string userId, IEnumerable<int> categoryIds);

        Task<IEnumerable<Contact>> GetContactsAsync(string userId);
    }
}