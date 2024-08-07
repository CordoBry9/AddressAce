﻿using AddressAce.Client.Models;

namespace AddressAce.Client.Services.Interfaces
{
    public interface IContactDTOService
    {
        Task<ContactDTO> CreateContactAsync(ContactDTO contactDTO, string userId);

        Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId);

        Task<IEnumerable<ContactDTO>> GetContactsByCategoryIdAsync(int categoryId, string userId);

        Task UpdateContactAsync(ContactDTO contactDTO, string userId);

        Task<ContactDTO?> GetContactByIdAsync(int contactId, string userId);

        Task DeleteContactAsync(int contactId, string userId);

        Task<IEnumerable<ContactDTO>> SearchContactsAsync(string searchTerm, string userId);

        Task<bool> EmailContactAsync(int contactId, EmailData emailData, string userId);
    }
}
