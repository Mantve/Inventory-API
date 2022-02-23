using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> Create(Category category);
        Task Delete(Category category);
        Task<Category> Get(int id, string username);
        Task<IEnumerable<Category>> GetAll(string username);
        Task<Category> Put(Category category);
    }

    public class CategoryRepository : ICategoryRepository
    {
        private readonly RestContext _restContext;

        public CategoryRepository(RestContext restContext)
        {
            _restContext = restContext;
        }

        public async Task<Category> Create(Category category)
        {
            _restContext.Categories.Add(category);
            await _restContext.SaveChangesAsync();

            return category;
        }

        public async Task<Category> Get(int id, string username)
        {
            return await _restContext.Categories.FirstOrDefaultAsync(x => x.Id == id && x.Author.Username == username);
        }

        public async Task<IEnumerable<Category>> GetAll(string username)
        {
            return await _restContext.Categories.Where(x => x.Author.Username == username).ToListAsync();
        }

        public async Task<Category> Put(Category category)
        {
            _restContext.Categories.Update(category);
            await _restContext.SaveChangesAsync();
            return category;
        }

        public async Task Delete(Category category)
        {
            _restContext.Categories.Remove(category);
            await _restContext.SaveChangesAsync();
        }
    }
}
