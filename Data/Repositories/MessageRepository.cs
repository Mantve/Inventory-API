using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface IMessageRepository
    {
        Task<Message> Create(Message message);
        Task Delete(Message message);
        Task<Message> Get(int id, string username);
        Task<IEnumerable<Message>> GetAll(string username);
        Task<Message> Put(Message message);
    }

    public class MessageRepository : IMessageRepository
    {
        private readonly RestContext _restContext;

        public MessageRepository(RestContext restContext)
        {
            _restContext = restContext;
        }

        public async Task<Message> Create(Message message)
        {
            _restContext.Messages.Add(message);
            await _restContext.SaveChangesAsync();

            return message;
        }

        public async Task<Message> Get(int id, string username)
        {
            return await _restContext.Messages.FirstOrDefaultAsync(x => x.Id == id && x.Author.Username == username);
        }

        public async Task<IEnumerable<Message>> GetAll(string username)
        {
            return await _restContext.Messages.Where(x => x.Author.Username == username).ToListAsync();
        }

        public async Task<Message> Put(Message message)
        {
            _restContext.Messages.Update(message);
            await _restContext.SaveChangesAsync();
            return message;
        }

        public async Task Delete(Message message)
        {
            _restContext.Messages.Remove(message);
            await _restContext.SaveChangesAsync();
        }
    }
}
