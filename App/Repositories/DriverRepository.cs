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
        public async Task<List<Driver>> GetFilteredDriversAsync(string name = null, string phoneNumber = null, string licensePlate = null)
        {
            var query = _context.Drivers.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.FullName.Contains(name));
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                query = query.Where(c => c.PhoneNumber.Contains(phoneNumber));
            }

            if (!string.IsNullOrEmpty(licensePlate))
            {
                query = query.Where(c => c.LicensePlate.Contains(licensePlate));
            }

            return await query.ToListAsync();
        }

        public async Task<List<Driver>> GetAllDriversAsync()
        {
            return await _context.Drivers
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
