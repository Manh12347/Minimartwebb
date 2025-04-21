using System.Text.RegularExpressions;
using MinimartWeb.Model;
using MinimartWeb.DAOs;

namespace MinimartWeb.BOs
{
    public class CategoryBO
    {
        private readonly CategoryDAO _categoryDAO;

        public CategoryBO(CategoryDAO categoryDAO)
        {
            _categoryDAO = categoryDAO;
        }

        public async Task<(bool, List<string>)> ValidateAsync(Category category, bool isEdit = false)
        {
            List<string> errors = new();

            if (string.IsNullOrWhiteSpace(category.CategoryName) || category.CategoryName.Length > 255)
                errors.Add("Category name is required and must not exceed 255 characters.");

            if (!Regex.IsMatch(category.CategoryName ?? "", @"^[A-Za-z0-9\s\-']+$"))
                errors.Add("Category name contains invalid characters. Only letters, numbers, spaces, hyphens, and apostrophes are allowed.");

            if (await _categoryDAO.ExistsByNameAsync(category.CategoryName, isEdit ? category.CategoryID : null))
                errors.Add("A category with this name already exists.");

            return (!errors.Any(), errors);
        }

        public Task<List<Category>> GetAllAsync() => _categoryDAO.GetAllAsync();

        public Task<Category?> GetByIdAsync(int id) => _categoryDAO.GetByIdAsync(id);

        public async Task<(bool, List<string>)> AddAsync(Category category)
        {
            var (valid, errors) = await ValidateAsync(category);
            if (!valid) return (false, errors);

            await _categoryDAO.AddAsync(category);
            return (true, new());
        }

        public async Task<(bool, List<string>)> UpdateAsync(Category category)
        {
            var (valid, errors) = await ValidateAsync(category, isEdit: true);
            if (!valid) return (false, errors);

            await _categoryDAO.UpdateAsync(category);
            return (true, new());
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryDAO.GetByIdAsync(id);
            if (category != null)
                await _categoryDAO.DeleteAsync(category);
        }
    }
}
