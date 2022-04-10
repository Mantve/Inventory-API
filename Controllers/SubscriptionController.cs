using AutoMapper;
using Inventory_API.Data.Dtos.Subscription;
using Inventory_API.Data.Entities;
using Inventory_API.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Inventory_API.Controllers
{
    [ApiController]
    [Route("api/subscription")]
    public class SubscriptionController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IMapper _mapper;

        public SubscriptionController(IUserRepository userRepository, ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult<SubscriptionDto>> Post(CreateSubscriptionDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            var user = await _userRepository.GetByUsername(username);
            var subscription = _mapper.Map<Subscription>(dto);
            subscription.User = user;
            await _subscriptionRepository.Create(subscription);
            return Ok(_mapper.Map<SubscriptionDto>(subscription));
        }
    }
}
