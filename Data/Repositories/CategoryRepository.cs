using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {

    }

    public class CategoryRepository :  GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(RestContext restContext) : base(restContext)
        {
            _restContext = restContext;
        }

    }
}
