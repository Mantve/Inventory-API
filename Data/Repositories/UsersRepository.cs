using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public class UsersRepository
    {
        public interface IUserRepository
        {
            User Create(User user);
            User GetByUsername(string username);
            User GetById(int id);
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

            public User Create(User user)
            {
                RestContext.Users.Add(user);
                user.Id = RestContext.SaveChanges();

                return user;
            }

            public User GetByUsername(string username)
            {
                return RestContext.Users.FirstOrDefault(u => u.Username == username);
            }

            public User GetById(int id)
            {
                return RestContext.Users.FirstOrDefault(u => u.Id == id);
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
}
