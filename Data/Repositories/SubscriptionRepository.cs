using Inventory_API.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_API.Data.Repositories
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription>
    {
        Task<IEnumerable<Subscription>> GetAll(string username);
    }

    public class SubscriptionRepository : GenericRepository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(RestContext restContext) : base(restContext)
        {
            _restContext = restContext;
        }

        public async Task<IEnumerable<Subscription>> GetAll(string username)
        {
            return await _restContext.Subscriptions.Where(s => s.User.Username == username).ToListAsync();
        }

    }
}
