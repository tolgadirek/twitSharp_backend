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
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public IActionResult Register(UserRegisterDto registerDto)
        {
            var user = new User { Email = registerDto.Email };

            var result = _authService.Register(user, registerDto.Password, registerDto.FirstName, registerDto.LastName);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDto { Message = result.Message });
            }

            var tokenResult = _authService.CreateAccessToken(result.Data);
            if (!tokenResult.Success)
            {
                return BadRequest(new ErrorResponseDto { Message = tokenResult.Message });
            }

            var refreshTokenResult = _authService.CreateRefreshToken(result.Data);

            return Ok(new
            {
                token = tokenResult.Data,
                refreshToken = refreshTokenResult.Data,
                user = result.Data
            });
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDto loginDto)
        {
            var userToLogin = _authService.Login(loginDto.Email, loginDto.Password);
            if (!userToLogin.Success)
            {
                return BadRequest(new ErrorResponseDto { Message = userToLogin.Message });
            }

            var tokenResult = _authService.CreateAccessToken(userToLogin.Data);
            if (!tokenResult.Success)
            {
                return BadRequest(new ErrorResponseDto { Message = tokenResult.Message });
            }

            var refreshTokenResult = _authService.CreateRefreshToken(userToLogin.Data);

            return Ok(new
            {
                token = tokenResult.Data,
                refreshToken = refreshTokenResult.Data,
                user = userToLogin.Data
            });
        }

        [Authorize]
        [HttpGet("profile")]
        public IActionResult GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var result = _authService.GetProfile(userId);
            if (!result.Success)
            {
                return NotFound(new ErrorResponseDto { Message = result.Message });
            }

            return Ok(result.Data);
        }

        [Authorize]
        [HttpPut("update")]
        public IActionResult UpdateProfile([FromBody] UserUpdateDto updateDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var updatedUser = new User
            {
                UserId = userId,
                FirstName = updateDto.FirstName,
                LastName = updateDto.LastName,
                Email = updateDto.Email,
            };

            var result = _authService.UpdateProfile(updatedUser, updateDto.Password);
            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDto { Message = result.Message });
            }

            return Ok(result.Data);
        }

        [HttpPost("refresh")]
        public IActionResult Refresh([FromBody] TokenRefreshDto dto)
        {
            var result = _authService.RefreshAccessToken(dto.RefreshToken);

            if (!result.Success)
            {
                return BadRequest(new ErrorResponseDto { Message = result.Message });
            }

            return Ok(result.Data);
        }
    }
}
