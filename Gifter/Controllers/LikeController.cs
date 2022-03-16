using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gifter.Repositories;

namespace Gifter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        private readonly ILikeRepository _likeRepo;

        public LikeController(ILikeRepository likeRepo)
        {
            _likeRepo = likeRepo;
        }

        [HttpPost]
        public IActionResult Post(int postId)
        {
            _likeRepo.Add(postId);
            return NoContent();
        }
    }
}
