using SmartSpend.API.DTOs;

namespace SmartSpend.API.Interfaces
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAll();
        Task<List<CategoryDto>> GetByType(string type);
        Task<CategoryDto> Add(AddCategoryDto dto);
        Task<bool> Delete(int categoryId);
    }
}