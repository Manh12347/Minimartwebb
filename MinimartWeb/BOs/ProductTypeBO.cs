using MinimartWeb.Model;
using MinimartWeb.DAOs;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MinimartWeb.BOs
{
    public class ProductTypeBO
    {
        private readonly ProductTypeDAO _dao;

        public ProductTypeBO(ProductTypeDAO dao)
        {
            _dao = dao;
        }

        public async Task<List<ProductType>> GetAllAsync() => await _dao.GetAllAsync();

        public async Task<ProductType?> GetByIdAsync(int id) => await _dao.GetByIdAsync(id);

        public List<Category> GetCategories() => _dao.GetCategories();
        public List<Supplier> GetSuppliers() => _dao.GetSuppliers();
        public List<MeasurementUnit> GetMeasurementUnits() => _dao.GetMeasurementUnits();

        public async Task<(bool IsSuccess, string Message)> CreateAsync(ProductType product)
        {
            var validation = ValidateProduct(product, null);
            if (!validation.IsSuccess) return (false, validation.Message);

            await _dao.AddAsync(product);
            return (true, "Product created successfully.");
        }

        public async Task<(bool IsSuccess, string Message)> UpdateAsync(int id, ProductType product)
        {
            if (!_dao.ProductTypeExists(id))
                return (false, "Product not found.");

            var validation = ValidateProduct(product, id);
            if (!validation.IsSuccess) return (false, validation.Message);

            await _dao.UpdateAsync(product);
            return (true, "Product updated successfully.");
        }

        public async Task<(bool IsSuccess, string Message)> DeleteAsync(int id)
        {
            var product = await _dao.GetByIdAsync(id);
            if (product == null)
                return (false, "Product not found.");

            await _dao.DeleteAsync(product);
            return (true, "Product deleted.");
        }

        private (bool IsSuccess, string Message) ValidateProduct(ProductType product, int? excludeId)
        {
            if (string.IsNullOrWhiteSpace(product.ProductName) || product.ProductName.Length > 255)
                return (false, "Product name is required and must be less than 255 characters.");

            if (_dao.ProductNameExists(product.ProductName, excludeId))
                return (false, "Product name must be unique.");

            if (string.IsNullOrWhiteSpace(product.ProductDescription))
                return (false, "Product description is required.");

            if (product.Price < 0)
                return (false, "Price cannot be negative.");

            if (product.StockAmount < 0)
                return (false, "Stock amount cannot be negative.");

            if (!_dao.CategoryExists(product.CategoryID))
                return (false, "Invalid Category selected.");

            if (!_dao.SupplierExists(product.SupplierID))
                return (false, "Invalid Supplier selected.");

            if (!_dao.MeasurementUnitExists(product.MeasurementUnitID))
                return (false, "Invalid Measurement Unit selected.");

            if (!string.IsNullOrEmpty(product.ImagePath) && product.ImagePath.Length > 512)
                return (false, "Image path cannot exceed 512 characters.");

            return (true, "Validation passed.");
        }
    }
}
