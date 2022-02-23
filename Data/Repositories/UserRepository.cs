using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{

    public interface IUserRepository
        {
            Task<User> Create(User user);
            Task<User> GetByUsername(string username);
            Task<User> Put(User user);
            Task<IEnumerable<User>> GetByRole(string role);
        }

        public class UserRepository : IUserRepository
        {
            private readonly RestContext _restContext;

            public UserRepository(RestContext restContext)
            {
                _restContext = restContext;
            }

            public async Task<User> Create(User user)
            {
                _restContext.Users.Add(user);
                await _restContext.SaveChangesAsync();

                return user;
            }

            public async Task<User> GetByUsername(string username)
            {
                return await _restContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            }


            public async Task<User> Put(User user)
            {
                _restContext.Users.Update(user);
                await _restContext.SaveChangesAsync();
                return user;
            }

            public async Task<IEnumerable<User>> GetByRole(string role)
            {
                return await _restContext.Users.Where(o => o.Role == role || role == null).ToListAsync();
            }
        }
    
}
