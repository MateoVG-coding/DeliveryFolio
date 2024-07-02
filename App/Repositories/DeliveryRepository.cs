using Courier_Data_Control_App.Classes;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Data_Control_App.Repositories
{
    public class DeliveryRepository
    {
        private readonly AppDbContext _context;

        public DeliveryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Delivery>> GetFilteredDeliveriesAsync(string customerName = null, string phoneNumber = null,
                                                                    string address = null, string description = null)
        {
            var query = _context.Deliveries.AsQueryable();

            if (!string.IsNullOrEmpty(customerName))
            {
                query = query.Where(c => c.CustomerName.Contains(customerName));
            }

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                query = query.Where(c => c.PhoneNumber.Contains(phoneNumber));
            }

            if (!string.IsNullOrEmpty(address))
            {
                query = query.Where(c => c.Address.Contains(address));
            }

            if (!string.IsNullOrEmpty(description))
            {
                query = query.Where(c => c.Description.Contains(description));
            }

            return await query.ToListAsync();
        }

        public async Task<List<Delivery>> GetAllDeliveriesAsync()
        {
            return await _context.Deliveries.Include(d => d.Driver).ToListAsync();
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
