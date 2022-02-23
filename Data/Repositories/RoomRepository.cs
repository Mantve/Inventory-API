using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly RestContext RestContext;

        public RoomRepository(RestContext restContext)
        {
            RestContext = restContext;
        }

        public async Task<Room> Create(Room room)
        {
            RestContext.Rooms.Add(room);
            await RestContext.SaveChangesAsync();

            return room;
        }

        public async Task<Room> Get(int id, string username)
        {
            return await RestContext.Rooms.FirstOrDefaultAsync(x => x.Id == id && x.Author.Username == username);
        }

        public async Task<IEnumerable<Room>> GetAll(string username)
        {
            return await RestContext.Rooms.Where(x => x.Author.Username == username).ToListAsync();
        }

        public async Task<Room> Put(Room room)
        {
            RestContext.Rooms.Update(room);
            await RestContext.SaveChangesAsync();
            return room;
        }

        public async Task Delete(Room room)
        {
            RestContext.Rooms.Remove(room);
            await RestContext.SaveChangesAsync();
        }
    }
}

