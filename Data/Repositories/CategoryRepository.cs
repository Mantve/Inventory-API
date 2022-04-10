using Inventory_API.Data.Entities;

namespace Inventory_API.Data.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {

    }

    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(RestContext restContext) : base(restContext)
        {
            _restContext = restContext;
        }

    }
}
