using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface IListRepository : IGenericRepository<List>
    {
        Task<List> Get(int id, string username);
        Task<IEnumerable<List>> GetAll(string username);
    }

    public class ListRepository : GenericRepository<List>, IListRepository
    {

        public ListRepository(RestContext restContext) : base(restContext)
        {
            _restContext = restContext;
        }


        public async Task<List> Get(int id, string username)
        {
            return await _restContext.Lists.Include(x => x.Items).ThenInclude(x => x.Item).FirstOrDefaultAsync(x => x.Id == id && x.Author.Username == username);
        }

        public async Task<IEnumerable<List>> GetAll(string username)
        {
            return await _restContext.Lists.Where(x => x.Author.Username == username).ToListAsync();
        }

    }
}
