using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task<Item> Get(int id, string username);
        Task<Item> Get(int id);
        Task<Item> GetParent(Item item);
        Task<IEnumerable<Item>> GetAll(string username);
        Task<IEnumerable<Item>> GetAll(int roomId);
        Task<IEnumerable<Item>> GetAllRecursive(int roomId);
    }

    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {

        public ItemRepository(RestContext restContext) : base(restContext)
        {
            _restContext = restContext;
        }

        public async Task<Item> Get(int id, string username)
        {
            return await _restContext.Items.Include(x => x.Room).ThenInclude(x => x.SharedWith).Include(x => x.Items).Include(x=> x.Category).FirstOrDefaultAsync(x => x.Id == id && x.Room.SharedWith.Any(y => y.Username == username));
        }

        public async Task<Item> Get(int id)
        {
            return await _restContext.Items.Include(x=>x.ParentItem).Include(x => x.Room).ThenInclude(x => x.SharedWith).Include(x=>x.Items).ThenInclude(x => x.Items).ThenInclude(x => x.Items).ThenInclude(x => x.Items).ThenInclude(x => x.Items).Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Item>> GetAll(string username)
        {
            return await _restContext.Items.Include(x => x.Room).ThenInclude(x => x.SharedWith).Include(x => x.Items).Include(x => x.Category).Where(x => x.Room.SharedWith.Any(y => y.Username == username)).ToListAsync();
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

    }
}
