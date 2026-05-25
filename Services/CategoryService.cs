using Microsoft.EntityFrameworkCore;
using SmartSpend.API.Data;
using SmartSpend.API.DTOs;
using SmartSpend.API.Interfaces;
using SmartSpend.API.Models;

namespace SmartSpend.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly SmartSpendDbContext _context;

        public CategoryService(SmartSpendDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryDto>> GetAll()
        {
            return await _context.Categories
                .OrderBy(c => c.Type)
                .ThenBy(c => c.Name)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Type = c.Type,
                    IsDefault = c.IsDefault
                })
                .ToListAsync();
        }

        public async Task<List<CategoryDto>> GetByType(string type)
        {
            return await _context.Categories
                .Where(c => c.Type == type)
                .OrderBy(c => c.Name)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Type = c.Type,
                    IsDefault = c.IsDefault
                })
                .ToListAsync();
        }

        public async Task<CategoryDto> Add(AddCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name,
                Type = dto.Type,
                IsDefault = false  // User created -- not default!
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Type = category.Type,
                IsDefault = category.IsDefault
            };
        }

        public async Task<bool> Delete(int categoryId)
        {
            // Don't allow deleting default categories!
            var category = await _context.Categories
                .FirstOrDefaultAsync(c =>
                    c.CategoryId == categoryId &&
                    !c.IsDefault);

            if (category == null) return false;

            // Check if category is used in transactions
            var isUsed = await _context.Transactions
                .AnyAsync(t => t.CategoryId == categoryId);

            if (isUsed)
                throw new Exception(
                    "Cannot delete category — it has transactions!");

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}