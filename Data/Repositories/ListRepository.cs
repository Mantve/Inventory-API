using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface IListRepository
    {
        Task<List> Create(List list);
        Task Delete(List list);
        Task<List> Get(int id, string username);
        Task<IEnumerable<List>> GetAll(string username);
        Task<List> Put(List list);
    }

    public class ListRepository : IListRepository
    {

        private readonly RestContext _restContext;

        public ListRepository(RestContext restContext)
        {
            _restContext = restContext;
        }

        public async Task<List> Create(List list)
        {
            _restContext.Lists.Add(list);
            await _restContext.SaveChangesAsync();

            return list;
        }

        public async Task<List> Get(int id, string username)
        {
            return await _restContext.Lists.Include(x => x.Items).ThenInclude(x => x.Item).FirstOrDefaultAsync(x => x.Id == id && x.Author.Username == username);
        }

        public async Task<IEnumerable<List>> GetAll(string username)
        {
            return await _restContext.Lists.Where(x => x.Author.Username == username).ToListAsync();
        }

        public async Task<List> Put(List list)
        {
            _restContext.Lists.Update(list);
            await _restContext.SaveChangesAsync();
            return list;
        }

        public async Task Delete(List list)
        {
            _restContext.Lists.Remove(list);
            await _restContext.SaveChangesAsync();
        }

    }
}
