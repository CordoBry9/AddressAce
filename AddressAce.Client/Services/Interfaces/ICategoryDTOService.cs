using AddressAce.Client.Models;

namespace AddressAce.Client.Services.Interfaces
{
    public interface ICategoryDTOService
    {

        Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId);

        Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId);

        Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId, string userId);

        Task DeleteCategoryAsync(int categoryId, string userId);

        Task UpdateCategoryAsync(int categoryId, CategoryDTO category, string userId);

        Task<bool> EmailCategoryAsync(int categoryId, EmailData emailData, string userId);
        
    }
}