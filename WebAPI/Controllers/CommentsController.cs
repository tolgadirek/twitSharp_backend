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
    public class CommentsController : ControllerBase
    {
        private readonly IPostCommentService _commentService;

        public CommentsController(IPostCommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public IActionResult AddComment(CreateCommentDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var result = _commentService.AddComment(userId, dto);

            if (!result.Success)
                return BadRequest(new ErrorResponseDto { Message = result.Message });

            return Ok(result.Data);
        }

        [HttpGet("{postId}")]
        public IActionResult GetComments(int postId)
        {
            var result = _commentService.GetComments(postId);

            if (!result.Success)
                return BadRequest(new ErrorResponseDto { Message = result.Message });

            return Ok(result.Data);
        }
    }
}

