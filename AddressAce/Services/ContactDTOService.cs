using AddressAce.Client.Models;
using AddressAce.Client.Services.Interfaces;
using AddressAce.Helpers;
using AddressAce.Models;
using AddressAce.Services.Interfaces;

namespace AddressAce.Services
{
    public class ContactDTOService(IContactRepository repository) : IContactDTOService
    {
        public async Task<ContactDTO> CreateContactAsync(ContactDTO contactDTO, string userId)
        {
            //Translate the DTO into a contact
            Contact newContact = new Contact() //inline instantiation
            {
                FirstName = contactDTO.FirstName,
                LastName = contactDTO.LastName,
                Email = contactDTO.Email,
                Address1 = contactDTO.Address1,
                Address2 = contactDTO.Address2,
                City = contactDTO.City,
                State = contactDTO.State,
                ZipCode = contactDTO.ZipCode,
                PhoneNumber = contactDTO.PhoneNumber,
                BirthDate = contactDTO.BirthDate,
                Created = DateTimeOffset.Now,
                AppUserId = userId,
                
            };
            //TODO images
            if (contactDTO.ImageUrl?.StartsWith("data:") == true)
            {
                newContact.Image = UploadHelper.GetImageUpload(contactDTO.ImageUrl);
            }

            //send to repository
            Contact createdContact = await repository.CreateContactAsync(newContact);
            //TODO: categories
            IEnumerable<int> categoryIds = contactDTO.Categories.Select(c => c.Id);
            await repository.AddCategoriesToContactAsync(createdContact.Id, userId, categoryIds);
            //return created DTO
            return createdContact.ToDTO();

        }

        public async Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId)
        {
            IEnumerable<Contact> contacts = await repository.GetContactsAsync(userId);

            IEnumerable<ContactDTO> contactDTOs = contacts.Select(c => c.ToDTO());

            return contactDTOs;
        }
    }
}
