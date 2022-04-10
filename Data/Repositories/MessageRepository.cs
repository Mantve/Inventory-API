using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface IMessageRepository : IGenericRepository<Message>
    {
        Task<Message> Get(int id, string username);
        Task<IEnumerable<Message>> GetAll(string username);
        Task<IEnumerable<Message>> GetAllCreated(string username);
        Task<IEnumerable<Message>> GetAllType(string username, MessageType messageType);
        Task<Message> GetCreated(int id, string username);
        Task<IEnumerable<Message>> GetAll(string author, string recipient, MessageType type);
    }

    public class MessageRepository : GenericRepository<Message>, IMessageRepository
    {

        public MessageRepository(RestContext restContext) : base(restContext)
        {
            _restContext = restContext;
        }

        public async Task<Message> GetCreated(int id, string username)
        {
            return await _restContext.Messages.FirstOrDefaultAsync(x => x.Id == id && x.Author.Username == username);
        }

        public async Task<IEnumerable<Message>> GetAllCreated(string username)
        {
            return await _restContext.Messages.Where(x => x.Author.Username == username).ToListAsync();
        }

        public async Task<Message> Get(int id, string username)
        {
            return await _restContext.Messages.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id && x.Recipient.Username == username);
        }

        public async Task<IEnumerable<Message>> GetAll(string author, string recipient, MessageType type)
        {
            return await _restContext.Messages.Include(x => x.Author).Where(x => x.MessageType == type && x.Author.Username == author && x.Recipient.Username == recipient).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetAll(string username)
        {
            return await _restContext.Messages.Include(x => x.Author).Where(x => x.Recipient.Username == username).ToListAsync();
        }

        public async Task<IEnumerable<Message>> GetAllType(string username, MessageType messageType)
        {
            return await _restContext.Messages.Include(x => x.Author).Where(x => x.Recipient.Username == username && x.MessageType == messageType).ToListAsync();
        }

    }
}
