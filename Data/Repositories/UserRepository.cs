using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
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
            private readonly RestContext RestContext;

            public UserRepository(RestContext restContext)
            {
                RestContext = restContext;
            }

            public async Task<User> Create(User user)
            {
                RestContext.Users.Add(user);
                await RestContext.SaveChangesAsync();

                return user;
            }

            public async Task<User> GetByUsername(string username)
            {
                return await RestContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            }


            public async Task<User> Put(User user)
            {
                RestContext.Users.Update(user);
                await RestContext.SaveChangesAsync();
                return user;
            }

            public async Task<IEnumerable<User>> GetByRole(string role)
            {
                return await RestContext.Users.Where(o => o.Role == role || role == null).ToListAsync();
            }
        }
    
}
