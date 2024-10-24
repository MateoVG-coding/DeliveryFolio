using Courier_Data_Control_App.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Courier_Data_Control_App.Repositories
{
    public class DeliveryRepository
    {
        private readonly AppDbContext _context;

        public DeliveryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalDeliveriesCountAsync(string searchFilter, DateTime? timeSpan)
        {
            var query = _context.Deliveries.AsQueryable();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                query = query.Where(d =>
                    d.CustomerName.Contains(searchFilter) ||
                    d.PhoneNumber.Contains(searchFilter) ||
                    d.Driver.FullName.Contains(searchFilter));
            }

            //Apply date range filter if provided
            if (timeSpan.HasValue)
            {
                query = query.Where(d => d.DateCreated >= timeSpan && d.DateCreated <= DateTime.Today.AddDays(1).AddTicks(-1));
            }

            return await query.CountAsync();
        }

        public async Task<List<Delivery>> GetAllDeliveriesAsync(int pageNumber, int pageSize, string searchFilter, DateTime? timeSpan)
        {
            var query = _context.Deliveries.Include(d => d.Driver).AsQueryable();

            if (!string.IsNullOrEmpty(searchFilter))
            {
                query = query.Where(d =>
                    d.CustomerName.Contains(searchFilter) ||
                    d.PhoneNumber.Contains(searchFilter) ||
                    d.Driver.FullName.Contains(searchFilter));
            }

            //Apply date range filter if provided
            if (timeSpan.HasValue)
            {
                query = query.Where(d => d.DateCreated >= timeSpan && d.DateCreated <= DateTime.Today.AddDays(1).AddTicks(-1));
            }

            return await query
            .OrderByDescending(d => d.DateCreated)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        }

        public async Task AddDeliveryAsync(Delivery delivery)
        {
            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDeliveryAsync(Delivery delivery)
        {
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDeliveryAsync(Delivery delivery)
        {
            _context.Deliveries.Remove(delivery);
            await _context.SaveChangesAsync();
        }
    }
}
