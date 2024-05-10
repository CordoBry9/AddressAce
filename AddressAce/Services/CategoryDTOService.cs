﻿using AddressAce.Client.Models;
using AddressAce.Client.Services.Interfaces;
using AddressAce.Models;
using AddressAce.Services.Interfaces;

namespace AddressAce.Services
{
    public class CategoryDTOService(ICategoryRepository repository) : ICategoryDTOService
    {
        public async Task<CategoryDTO> CreateCategoryAsync(CategoryDTO category, string userId)
        {
            Category newCategory = new Category()
            {
                Name = category.Name,
                AppUserId = userId,
            };

            Category createdCategory = await repository.CreateCategoryAsync(newCategory);

            return createdCategory.ToDTO();
        }

        public async Task DeleteCategoryAsync(int categoryId, string userId)
        {
            await repository.DeleteCategoryAsync(categoryId, userId);
        }

        public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync(string userId)
        {
            IEnumerable<Category> categories = await repository.GetCategoriesAsync(userId);

            IEnumerable<CategoryDTO> categoryDTOs = categories.Select(c => c.ToDTO());

            return categoryDTOs;
        }

        public async Task<CategoryDTO?> GetCategoryByIdAsync(int categoryId, string userId)
        {
            Category? category = await repository.GetCategoryByIdAsync(categoryId, userId);
            return category?.ToDTO();
        }

        public async Task UpdateCategoryAsync(CategoryDTO category, string userId)
        {
            Category? categoryToUpdate = await repository.GetCategoryByIdAsync(category.Id, userId);

            if (categoryToUpdate != null)
            { 
                categoryToUpdate.Name = category.Name;

                await repository.UpdateCategoryAsync(categoryToUpdate, userId);
            }
        }
    }
}