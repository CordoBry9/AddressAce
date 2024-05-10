using AddressAce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AddressAce.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        public virtual DbSet<ImageUpload> Images { get; set; }

        public virtual DbSet<Contact> Contacts { get; set; }

        public DbSet<Category> Categories { get; set; }

    }
}
