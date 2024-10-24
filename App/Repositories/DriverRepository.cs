using Courier_Data_Control_App.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Data_Control_App.Repositories
{
    public class DriverRepository
    {
        private readonly AppDbContext _context;

        public DriverRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Driver>> GetAllDriversAsync(string driverName)
        {
            var query = _context.Drivers.AsQueryable();

            if (!string.IsNullOrEmpty(driverName))
            {
                query = query.Where(c => c.FullName.Contains(driverName));
            }

            return await query
                .Where(d => d.IsInCompany.Equals(true))
                .OrderBy(d => d.FullName) 
                .ToListAsync();
        }

        public async Task AddDriverAsync(Driver driver)
        {
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDriverAsync(Driver driver)
        {
            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDriverAsync(Driver driver)
        {
            _context.Drivers.Remove(driver);
            await _context.SaveChangesAsync();
        }
    }
}
