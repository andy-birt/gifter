using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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
        public IActionResult Post(Dictionary<string, int> like)
        {
            int postId = like["postId"];
            _likeRepo.Add(postId);
            return NoContent();
        }
    }
}
