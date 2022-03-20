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
        Task<Item> Get(int id);
        Task<Item> GetParent(Item item);
        Task<IEnumerable<Item>> GetAll(string username);
        Task<IEnumerable<Item>> GetAll(int roomId);
        Task<IEnumerable<Item>> GetAllRecursive(int roomId);

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
        public async Task<Item> Get(int id, ICollection<string> usernames)
        {
            return await _restContext.Items.Include(x => x.Room).ThenInclude(x=> x.SharedWith).Include(x => x.Items).Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id && usernames.Contains(x.Author.Username));
        }

        public async Task<Item> Get(int id, string username)
        {
            return await _restContext.Items.Include(x => x.Room).ThenInclude(x => x.SharedWith).Include(x => x.Items).Include(x=> x.Category).FirstOrDefaultAsync(x => x.Id == id && x.Author.Username == username);
        }

        public async Task<Item> Get(int id)
        {
            return await _restContext.Items.Include(x=>x.ParentItem).Include(x => x.Room).ThenInclude(x => x.SharedWith).Include(x=>x.Items).ThenInclude(x => x.Items).ThenInclude(x => x.Items).ThenInclude(x => x.Items).ThenInclude(x => x.Items).Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Item>> GetAll(string username)
        {
            return await _restContext.Items.Include(x => x.Room).ThenInclude(x => x.SharedWith).Include(x => x.Items).Include(x => x.Category).Where(x => x.Author.Username == username).ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetAllRecursive(int roomId)
        {
            return await _restContext.Items.Include(x => x.ParentItem).Include(x => x.Category).Include(x => x.Room).ThenInclude(x => x.SharedWith).Include(x => x.Items).ThenInclude(x => x.Items).ThenInclude(x => x.Items).ThenInclude(x => x.Items).ThenInclude(x => x.Items).Include(x=> x.Category).Where(x => x.Room.Id == roomId && _restContext.Items.Where(y=>y.Items.Contains(x)).ToList().Count == 0).ToListAsync();
        }

        public async Task<IEnumerable<Item>> GetAll(int roomId)
        {
            return await _restContext.Items.Include(x => x.ParentItem).Include(x => x.Category).Include(x => x.Room).ThenInclude(x => x.SharedWith).Include(x => x.Category).Where(x => x.Room.Id == roomId).ToListAsync();
        }

        public async Task<Item> GetParent(Item item)
        {
            return await _restContext.Items.Include(x => x.ParentItem).Include(x => x.Category).Include(x => x.Room).ThenInclude(x => x.SharedWith).Include(x => x.Items).Include(x => x.Category).FirstOrDefaultAsync(x => x.Items.Contains(item));
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
