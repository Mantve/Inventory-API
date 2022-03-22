using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface IReminderRepository
    {
        Task<Reminder> Create(Reminder reminder);
        Task Delete(Reminder reminder);
        Task<Reminder> Get(int id, string username);
        Task<IEnumerable<Reminder>> GetAll(string username);
        Task<Reminder> Put(Reminder reminder);
    }

    public class ReminderRepository : IReminderRepository
    {
        private readonly RestContext _restContext;

        public ReminderRepository(RestContext restContext)
        {
            _restContext = restContext;
        }

        public async Task<Reminder> Create(Reminder reminder)
        {
            _restContext.Reminders.Add(reminder);
            await _restContext.SaveChangesAsync();

            return reminder;
        }

        public async Task<Reminder> Get(int id, string username)
        {
            return await _restContext.Reminders.FirstOrDefaultAsync(x => x.Id == id && x.Author.Username == username);
        }

        public async Task<IEnumerable<Reminder>> GetAll(string username)
        {
            return await _restContext.Reminders.Where(x => x.Author.Username == username).ToListAsync();
        }

        public async Task<Reminder> Put(Reminder reminder)
        {
            _restContext.Reminders.Update(reminder);
            await _restContext.SaveChangesAsync();
            return reminder;
        }

        public async Task Delete(Reminder reminder)
        {
            _restContext.Reminders.Remove(reminder);
            await _restContext.SaveChangesAsync();
        }
    }
}
