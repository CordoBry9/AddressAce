using AddressAce.Client.Models;
using AddressAce.Client.Services.Interfaces;
using AddressAce.Helpers;
using AddressAce.Models;
using AddressAce.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AddressAce.Services
{
    public class ContactDTOService(IContactRepository repository, IEmailSender emailSender) : IContactDTOService
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

        public async Task DeleteContactAsync(int contactId, string userId)
        {
            await repository.DeleteContactAsync(contactId, userId);
        }

        public async Task<bool> EmailContactAsync(int contactId, EmailData emailData, string userId)
        {
            try
            {
                Contact? contact = await repository.GetContactByIdAsync(contactId, userId);
                if (contact == null)
                {
                    return false;
                }
                else
                {
                    string recipient = contact.Email!;
                    await emailSender.SendEmailAsync(recipient, emailData.Subject!, emailData.Message!);

                    return true;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<ContactDTO?> GetContactByIdAsync(int contactId, string userId)
        {
            Contact? contact = await repository.GetContactByIdAsync(contactId, userId);

            return contact?.ToDTO();
            
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsAsync(string userId)
        {
            IEnumerable<Contact> contacts = await repository.GetContactsAsync(userId);

            IEnumerable<ContactDTO> contactDTOs = contacts.Select(c => c.ToDTO());

            return contactDTOs;
        }

        public async Task<IEnumerable<ContactDTO>> GetContactsByCategoryIdAsync(int categoryId, string userId)
        {
            IEnumerable<Contact> contacts = await repository.GetContactsByCategoryIdAsync(categoryId, userId);

            return contacts.Select(c => c.ToDTO());

            //List<ContactDTO> contactDTOs = [];

            //foreach (Contact contact in contacts)
            //{
            //    contactDTOs.Add(contact.ToDTO());
            //}
            //return contactDTOs;


        }

        public async Task<IEnumerable<ContactDTO>> SearchContactsAsync(string searchTerm, string userId)
        {
            IEnumerable<Contact> contacts = await repository.SearchContactsAsync(searchTerm, userId);

            return contacts.Select(c => c.ToDTO());
        }

        public async Task UpdateContactAsync(ContactDTO contactDTO, string userId)
        {
            Contact? contact = await repository.GetContactByIdAsync(contactDTO.Id, userId);

            if (contact is not null)
            {
                contact.FirstName = contactDTO.FirstName;
                contact.LastName = contactDTO.LastName;
                contact.Email = contactDTO.Email;   
                contact.Address1 = contactDTO.Address1;
                contact.Address2 = contactDTO.Address2;
                contact.BirthDate = contactDTO.BirthDate;
                contact.City = contactDTO.City;
                contact.State = contactDTO.State;
                contact.ZipCode = contactDTO.ZipCode;
                contact.PhoneNumber = contactDTO.PhoneNumber;

                if (contactDTO.ImageUrl.StartsWith("data:"))
                {
                    contact.Image = UploadHelper.GetImageUpload(contactDTO.ImageUrl);
                }
                else
                {
                    contact.Image = null;
                }
                //dont let the database update categories yet
                contact.Categories.Clear();

                await repository.UpdateContactAsync(contact);

                //remove the old categories
                await repository.RemoveCategoriesFromContactAsync(contact.Id, userId);

                //add back what the user selected
                IEnumerable<int> selectedCategoryIds = contactDTO.Categories.Select(c => c.Id);

                await repository.AddCategoriesToContactAsync(contact.Id, userId, selectedCategoryIds);

            }
        }
    }
}
