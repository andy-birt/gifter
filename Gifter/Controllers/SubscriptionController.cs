using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gifter.Repositories;
using System.Collections.Generic;

namespace Gifter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        [HttpPost]
        public IActionResult Post(int subscriberId, int providerId)
        {
            _subscriptionRepository.Add(subscriberId, providerId);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(int subscriberId, int providerId)
        {
            _subscriptionRepository.Delete(subscriberId, providerId);
            return NoContent();
        }
    }
}
