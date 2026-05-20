using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSpend.API.Data;
using SmartSpend.API.Helpers;

namespace SmartSpend.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : BaseController
    {
        private readonly SmartSpendDbContext _context;

        public CategoriesController(SmartSpendDbContext context, JwtHelper jwtHelper) : base(jwtHelper)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _context.Categories
                .Select(c => new
                {
                    c.CategoryId,
                    c.Name,
                    c.Type,
                    c.IsDefault
                })
                .ToListAsync();

            return Ok(categories);
        }

        [HttpGet("{type}")]
        public async Task<IActionResult> GetCategoriesByType(string type)
        {
            var categories = await _context.Categories
                .Where(c => c.Type == type)
                .Select(c => new
                {
                    c.CategoryId,
                    c.Name,
                    c.Type
                })
                .ToListAsync();

            return Ok(categories);
        }
    }
}