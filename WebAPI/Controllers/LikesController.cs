using Business.Abstract;
using Entity.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikesController : ControllerBase
    {
        private readonly IPostLikeService _likeService;

        public LikesController(IPostLikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("toggle")]
        public IActionResult ToggleLike([FromBody] LikePostDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = _likeService.ToggleLike(dto.PostId, userId);

            if (!result.Success)
                return BadRequest(new ErrorResponseDto { Message = result.Message });

            return Ok(new { message = result.Message });
        }

        [HttpGet("{postId}/count")]
        public IActionResult GetLikeCount(int postId)
        {
            var result = _likeService.GetLikeCount(postId);
            return Ok(result.Data);
        }
    }
}
