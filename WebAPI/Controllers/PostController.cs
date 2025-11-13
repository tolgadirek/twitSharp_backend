using Business.Abstract;
using Core.Utilities.Results;
using Entity.Concrete;
using Entity.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public IActionResult CreatePost([FromBody] PostCreateDto postCreateDto)
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var post = new Post
            {
                UserId = userId,
                Content = postCreateDto.Content,
                ImageUrl = postCreateDto.ImageUrl,
            };

            var result = _postService.Add(post);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDto { Message = result.Message });
            }

            return Ok(new { message = result.Message });
        }

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            var result = _postService.GetAll();

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDto { Message = result.Message });
            }

            return Ok(result.Data);
        }

        [HttpGet("me")]
        public IActionResult GetPostsByUserId()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = _postService.GetByUserId(userId);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDto { Message = result.Message });
            }

            return Ok(result.Data);
        }

        [HttpDelete("{postId}")]
        public IActionResult DeletePost(int postId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var result = _postService.Delete(postId, userId);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDto { Message = result.Message });
            }

            return Ok(new { message = result.Message });
        }
    }
}
