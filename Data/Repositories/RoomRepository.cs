using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{

    public interface IRoomRepository
    {
        Task<Room> Create(Room room);
        Task<Room> Get(int id, string username);
        Task<IEnumerable<Room>> GetAll(string username);
        Task<Room> Put(Room room);
        Task Delete(Room room);
    }

    public class RoomRepository : IRoomRepository
    {
        private readonly RestContext _restContext;

        public RoomRepository(RestContext restContext)
        {
            _restContext = restContext;
        }

        public async Task<Room> Create(Room room)
        {
            _restContext.Rooms.Add(room);
            await _restContext.SaveChangesAsync();

            return room;
        }

        public async Task<Room> Get(int id, string username)
        {
            return await _restContext.Rooms.FirstOrDefaultAsync(x => x.Id == id && x.SharedWith.Any( y => y.Username == username));
        }

        public async Task<IEnumerable<Room>> GetAll(string username)
        {
            return await _restContext.Rooms.Where(x => x.SharedWith.Any(y => y.Username == username)).ToListAsync();
        }

        public async Task<Room> Put(Room room)
        {
            _restContext.Rooms.Update(room);
            await _restContext.SaveChangesAsync();
            return room;
        }

        public async Task Delete(Room room)
        {
            _restContext.Rooms.Remove(room);
            await _restContext.SaveChangesAsync();
        }
    }
}

