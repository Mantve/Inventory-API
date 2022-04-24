using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{

    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User> GetByUsername(string username);
        Task<User> GetByEmail(string username);
        Task<User> GetFriends(string username);
        Task<IEnumerable<User>> GetByRole(string role);
    }

    public class UserRepository : GenericRepository<User>, IUserRepository
    {

        public UserRepository(RestContext restContext) : base(restContext)
        {
            _restContext = restContext;
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _restContext.Users.Include(x => x.Friends).FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User> GetByEmail(string email)
        {
            return await _restContext.Users.Include(x => x.Friends).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetFriends(string username)
        {
            return await _restContext.Users.Include(x => x.Friends).FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<User>> GetByRole(string role)
        {
            return await _restContext.Users.Where(o => o.Role == role || role == null).ToListAsync();
        }
    }

}
