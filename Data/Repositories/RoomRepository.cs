using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{

    public interface IRoomRepository : IGenericRepository<Room>
    {
        Task<Room> Get(int id, string username);
        Task<IEnumerable<Room>> GetAll(string username);
    }

    public class RoomRepository : GenericRepository<Room>, IRoomRepository
    {

        public RoomRepository(RestContext restContext) : base(restContext)
        {
            _restContext = restContext;
        }

        public async Task<Room> Get(int id, string username)
        {
            return await _restContext.Rooms.Include(x => x.Author).Include(x => x.SharedWith).FirstOrDefaultAsync(x => x.Id == id && x.SharedWith.Any(y => y.Username == username));
        }

        public async Task<IEnumerable<Room>> GetAll(string username)
        {
            return await _restContext.Rooms.Include(x => x.Author).Include(x => x.SharedWith).Where(x => x.SharedWith.Any(y => y.Username == username)).ToListAsync();
        }

    }
}

