using MinimartWeb.DAOs;
using MinimartWeb.Model;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MinimartWeb.BOs
{
    public class MeasurementUnitBO
    {
        private readonly MeasurementUnitDAO _dao;

        public MeasurementUnitBO(MeasurementUnitDAO dao)
        {
            _dao = dao;
        }

        public async Task<List<MeasurementUnit>> GetAllAsync() => await _dao.GetAllAsync();
        public async Task<MeasurementUnit?> GetByIdAsync(int id) => await _dao.GetByIdAsync(id);

        public async Task<(bool Success, List<string> Errors)> AddAsync(MeasurementUnit unit)
        {
            var errors = await ValidateAsync(unit);
            if (errors.Count > 0)
                return (false, errors);

            await _dao.AddAsync(unit);
            return (true, []);
        }

        public async Task<(bool Success, List<string> Errors)> UpdateAsync(MeasurementUnit unit)
        {
            var errors = await ValidateAsync(unit, isUpdate: true);
            if (errors.Count > 0)
                return (false, errors);

            await _dao.UpdateAsync(unit);
            return (true, []);
        }

        public async Task DeleteAsync(int id) => await _dao.DeleteAsync(id);

        private async Task<List<string>> ValidateAsync(MeasurementUnit unit, bool isUpdate = false)
        {
            List<string> errors = [];

            if (string.IsNullOrWhiteSpace(unit.UnitName))
                errors.Add("Unit name is required.");
            else if (unit.UnitName.Length > 50)
                errors.Add("Unit name must not exceed 50 characters.");
            else if (!Regex.IsMatch(unit.UnitName, @"^[\w\s\-]+$"))
                errors.Add("Unit name can only contain letters, numbers, spaces, hyphens, and underscores.");
            else if (await _dao.UnitNameExistsAsync(unit.UnitName, isUpdate ? unit.MeasurementUnitID : null))
                errors.Add("A unit with this name already exists.");

            return errors;
        }
    }
}
