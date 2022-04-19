using AutoMapper;
using Inventory_API.Data.Dtos.Subscription;
using Inventory_API.Data.Entities;
using Inventory_API.Data.Repositories;
using Inventory_API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;
        public SubscriptionController(IConfiguration configuration, IUserRepository userRepository, ISubscriptionRepository subscriptionRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _mapper = mapper;
            _configuration = configuration;
        }


        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Post(CreateSubscriptionDto dto)
        {
            string username = User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
            var user = await _userRepository.GetByUsername(username);
            var subscription = new Subscription();
            subscription.ExpirationTime = dto.ExpirationTime;
            var encryptionKey = System.Text.Encoding.UTF8.GetBytes(_configuration.GetValue<string>("encryptionKey"));
            var iv = System.Text.Encoding.UTF8.GetBytes(_configuration.GetValue<string>("iv"));
            subscription.Auth = CryptographyHelper.EncryptStringToBytes_Aes(dto.Auth, encryptionKey, iv);
            subscription.P256dh = CryptographyHelper.EncryptStringToBytes_Aes(dto.P256dh, encryptionKey, iv);
            subscription.Endpoint = CryptographyHelper.EncryptStringToBytes_Aes(dto.Endpoint, encryptionKey, iv);
            subscription.User = user;
            await _subscriptionRepository.Create(subscription);
            return Ok();
        }
    }
}
