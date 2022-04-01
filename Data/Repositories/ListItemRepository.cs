using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface IListItemRepository : IGenericRepository<ListItem>
    {

        Task<ListItem> Get(int id, string username);
        Task<IEnumerable<ListItem>> GetAll(string username);
    }

    public class ListItemRepository : GenericRepository<ListItem>, IListItemRepository
    {

        public ListItemRepository(RestContext restContext) : base(restContext)
        {
            _restContext = restContext;
        }

        public async Task<ListItem> Get(int id, string username)
        {
            return await _restContext.ListItems.Include(x => x.Item).Include(x => x.ParentList).FirstOrDefaultAsync(x => x.Id == id && x.ParentList.Author.Username == username);
        }

        public async Task<IEnumerable<ListItem>> GetAll(string username)
        {
            return await _restContext.ListItems.Where(x => x.ParentList.Author.Username == username).ToListAsync();
        }

    }
}
