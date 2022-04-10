using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface IReminderRepository : IGenericRepository<Reminder>
    {
        Task<Reminder> Get(int id, string username);
        Task<IEnumerable<Reminder>> GetAll(string username);
        Task<IEnumerable<Reminder>> GetAllDue();
    }

    public class ReminderRepository : GenericRepository<Reminder>, IReminderRepository
    {

        public ReminderRepository(RestContext restContext) : base(restContext)
        {
            _restContext = restContext;
        }

        public async Task<Reminder> Get(int id, string username)
        {
            return await _restContext.Reminders.Include(x => x.Item).ThenInclude(x => x.Room).FirstOrDefaultAsync(x => x.Id == id && x.Author.Username == username);
        }

        public async Task<IEnumerable<Reminder>> GetAll(string username)
        {
            return await _restContext.Reminders.Include(x => x.Item).Where(x => x.Author.Username == username).ToListAsync();
        }

        public async Task<IEnumerable<Reminder>> GetAllDue()
        {

            DateTime dateTime = System.DateTime.Now;
            return await _restContext.Reminders.Include(x => x.Item).Where(x => x.ReminderTime <= dateTime).ToListAsync();
        }
    }
}
