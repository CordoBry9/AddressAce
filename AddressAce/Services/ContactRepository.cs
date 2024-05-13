using AddressAce.Data;
using AddressAce.Models;
using AddressAce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AddressAce.Services
{
    public class ContactRepository(IDbContextFactory<ApplicationDbContext> contextFactory) : IContactRepository

    {
        public async Task AddCategoriesToContactAsync(int contactId, string userId, IEnumerable<int> categoryIds)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);

            if (contact != null)
            {
                foreach(int categoryId in categoryIds)
                {
                    Category? category = await context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId && c.AppUserId == userId);

                    if (category != null) 
                    { 

                        contact.Categories.Add(category);
                    }
                }
                await context.SaveChangesAsync();
                
            }
        }

        public async Task RemoveCategoriesFromContactAsync(int contactId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts
                .Include(c => c.Categories)
                .FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);

            if (contact != null)
            {
                contact.Categories.Clear();
                await context.SaveChangesAsync();
            }
        }

        public async Task<Contact> CreateContactAsync(Contact contact)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            context.Contacts.Add(contact);
            await context.SaveChangesAsync();

            return contact;
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync(string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            IEnumerable<Contact> contacts = await context.Contacts
                .Where(c => c.AppUserId == userId).Include(c => c.Categories).ToListAsync();

            return contacts;
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            bool shouldEdit = await context.Contacts.AnyAsync(c => c.AppUserId == contact.AppUserId && c.Id == contact.Id);

            if (shouldEdit)
            {

                //if theres a new immage
                //-save the image
                //-change the contact
                //-delete the old image
                ImageUpload? oldImage = null;
                if (contact.Image is not null)
                {
                    context.Images.Add(contact.Image); //Adds image to database table

                    oldImage = await context.Images.FirstOrDefaultAsync(i => i.Id == contact.ImageId); //checks for an old image

                    contact.ImageId = contact.Image.Id; //fix the foreign key
                }

                //tell the context we want to change this contact
                context.Contacts.Update(contact);
                // save changes
                await context.SaveChangesAsync();

                if(oldImage != null)
                {
                    context.Images.Remove(oldImage);
                    await context.SaveChangesAsync();
                }
            }

        }

        public async Task<Contact?> GetContactByIdAsync(int contactId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts.Include(c => c.Categories).FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);

            return contact;
        }

        public async Task DeleteContactAsync(int contactId, string userId)
        {
            using ApplicationDbContext context = contextFactory.CreateDbContext();

            Contact? contact = await context.Contacts.FirstOrDefaultAsync(c => c.Id == contactId && c.AppUserId == userId);

            if (contact is not null)
            {
                context.Contacts.Remove(contact);
                await context.SaveChangesAsync();
            };
        }
    }
}
