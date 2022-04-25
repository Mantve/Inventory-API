using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface IItemRepository : IGenericRepository<Item>
    {
        Task<Item> Get(int id, string username); // gets single item
        Task<IEnumerable<Item>> GetAll(int id, string username); // gets list of items
        Task<Item> GetRecursive(int id, string username); // gets tree of items
        Task<Item> GetParent(Item item);
        Task<IEnumerable<Item>> SearchAll(string username, string searchTerm); // gets list of items
        Task<IEnumerable<Item>> GetAllRecursiveFromRoom(int roomId, string username); // gets list of trees of items
        Task<IEnumerable<Item>> GetAllFromRoom(int roomId, string username); // gets list of items
    }

    public class ItemRepository : GenericRepository<Item>, IItemRepository
    {

        public ItemRepository(RestContext restContext) : base(restContext)
        {
            _restContext = restContext;
        }

        public async Task<Item> Get(int id, string username)
        {
            return await _restContext.Items
                .Include(x => x.Room)
                    .ThenInclude(x => x.SharedWith)
                .Include(x => x.Items)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id && x.Room.SharedWith.Any(y => y.Username == username));
        }

        public async Task<IEnumerable<Item>> GetAll(int id, string username)
        {
            var items = await _restContext.Items
                .Include(x => x.ParentItem)
                .Include(x => x.Room)
                    .ThenInclude(x => x.SharedWith)
                .Include(x => x.Items)
                .Include(x => x.Category)
                .Where(x => x.Id == id && x.Room.SharedWith.Any(y => y.Username == username)).ToListAsync();
            List<Item> res = items.ToList();
            foreach (var item in items)
            {
                foreach (var child in item.Items)
                {
                    res.AddRange(GetAll(child.Id, username).Result);

                }
            }
            return res;
        }

        public async Task<Item> GetRecursive(int id, string username)
        {
            var items = await _restContext.Items
                .Include(x => x.ParentItem)
                .Include(x => x.Room)
                    .ThenInclude(x => x.SharedWith)
                .Include(x => x.Items)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id && x.Room.SharedWith.Any(y => y.Username == username));
            foreach (var item in items.Items)
            {
                item.Items = GetRecursive(item.Id, username).Result.Items;
            }
            return items;
        }

        public async Task<IEnumerable<Item>> SearchAll(string username, string searchTerm)
        {
            return await _restContext.Items
                .Include(x => x.Room)
                    .ThenInclude(x => x.SharedWith)
                .Include(x => x.Items)
                .Include(x => x.Category)
                .Where(x => x.Room.SharedWith.Any(y => y.Username == username) && x.Name.Contains(searchTerm)).ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="username"></param>
        /// <returns>All items from the room in the tree structure/></returns>
        public async Task<IEnumerable<Item>> GetAllRecursiveFromRoom(int roomId, string username)
        {
            var items = await _restContext.Items
                .Include(x => x.ParentItem)
                .Include(x => x.Category)
                .Include(x => x.Room)
                    .ThenInclude(x => x.SharedWith)
                .Include(x => x.Items)
                .Include(x => x.Category)
                .Where(x => x.Room.Id == roomId // from the room
                && x.Room.SharedWith.Any(y => y.Username == username) // has permissions to the room
                && _restContext.Items.Where(y => y.Items.Contains(x)).ToList().Count == 0).ToListAsync(); // has no children (root item)
            foreach (var item in items)
            {
                item.Items = GetRecursive(item.Id, username).Result.Items;
            }
            return items;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns>All items from the room in the list structure</returns>
        public async Task<IEnumerable<Item>> GetAllFromRoom(int roomId, string username)
        {
            return await _restContext.Items
                .Include(x => x.ParentItem)
                .Include(x => x.Category)
                .Include(x => x.Room)
                    .ThenInclude(x => x.SharedWith)
                .Include(x => x.Category)
                .Where(x => x.Room.Id == roomId && x.Room.SharedWith.Any(y => y.Username == username)).ToListAsync();
        }

        public async Task<Item> GetParent(Item item)
        {
            return await _restContext.Items
                .Include(x => x.ParentItem)
                .Include(x => x.Category)
                .Include(x => x.Room)
                    .ThenInclude(x => x.SharedWith)
                .Include(x => x.Items)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Items.Contains(item));
        }

    }
}
