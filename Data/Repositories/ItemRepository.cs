using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface IItemRepository
    {
        Task<Item> Create(Item item);
        Task Delete(Item item);
        Task<Item> Get(int id, string username);
        Task<IEnumerable<Item>> GetAll(string username);
        Task<Item> Put(Item item);
    }

    public class ItemRepository : IItemRepository
    {
        private readonly RestContext _restContext;

        public ItemRepository(RestContext restContext)
        {
            _restContext = restContext;
        }

        public async Task<Item> Create(Item item)
        {
            _restContext.Items.Add(item);
            await _restContext.SaveChangesAsync();

            return item;
        }

        public async Task<Item> Get(int id, string username)
        {
            return await _restContext.Items.FirstOrDefaultAsync(x => x.Id == id && x.Author.Username == username);
        }

        public async Task<IEnumerable<Item>> GetAll(string username)
        {
            return await _restContext.Items.Where(x => x.Author.Username == username).ToListAsync();
        }

        public async Task<Item> Put(Item item)
        {
            _restContext.Items.Update(item);
            await _restContext.SaveChangesAsync();
            return item;
        }

        public async Task Delete(Item item)
        {
            _restContext.Items.Remove(item);
            await _restContext.SaveChangesAsync();
        }
    }
}
