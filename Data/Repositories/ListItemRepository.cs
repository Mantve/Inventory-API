using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface IListItemRepository
    {
        Task<ListItem> Create(ListItem listItem);
        Task Delete(ListItem listItem);
        Task<ListItem> Get(int id, string username);
        Task<IEnumerable<ListItem>> GetAll(string username);
        Task<ListItem> Put(ListItem listItem);
    }

    public class ListItemRepository : IListItemRepository
    {

        private readonly RestContext _restContext;

        public ListItemRepository(RestContext restContext)
        {
            _restContext = restContext;
        }

        public async Task<ListItem> Create(ListItem listItem)
        {
            _restContext.ListItems.Add(listItem);
            await _restContext.SaveChangesAsync();

            return listItem;
        }

        public async Task<ListItem> Get(int id, string username)
        {
            return await _restContext.ListItems.Include(x => x.Item).FirstOrDefaultAsync(x => x.Id == id && x.ParentList.Author.Username == username);
        }

        public async Task<IEnumerable<ListItem>> GetAll(string username)
        {
            return await _restContext.ListItems.Where(x => x.ParentList.Author.Username == username).ToListAsync();
        }

        public async Task<ListItem> Put(ListItem listItem)
        {
            _restContext.ListItems.Update(listItem);
            await _restContext.SaveChangesAsync();
            return listItem;
        }

        public async Task Delete(ListItem listItem)
        {
            _restContext.ListItems.Remove(listItem);
            await _restContext.SaveChangesAsync();
        }

    }
}
