using SmartSchool.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace SmartSchool.BAL
{
    public class ClientService : IClientService
    {
        private readonly MyDbContext _context;

        public ClientService(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<ClientEntity>> GetAllClientsAsync()
        {
            return await _context.Clients
                .Where(c => c.IsDeleted != true)
                .ToListAsync();
        }

        public async Task<ClientEntity> GetClientByIdAsync(int id)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.ClientId == id && c.IsDeleted != true);
        }

        public async Task<ClientEntity> AddClientAsync(ClientEntity client)
        {
            client.CreatedOn = DateTime.UtcNow;
            client.IsActive = true;
            client.IsDeleted = false;

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<ClientEntity> UpdateClientAsync(ClientEntity client)
        {
            var existing = await _context.Clients.FindAsync(client.ClientId);
            if (existing == null || existing.IsDeleted == true)
                return null;

            existing.FullName = client.FullName;
            existing.Email = client.Email;
            existing.Company = client.Company;
            existing.Phone = client.Phone;
            existing.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null || client.IsDeleted == true)
                return false;

            client.IsDeleted = true;
            client.IsActive = false;
            client.UpdatedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }

}
