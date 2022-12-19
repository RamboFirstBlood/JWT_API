using JWT_API.DTO;
using JWT_API.Models;
using JWT_API.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JWT_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new Models.User();
        private readonly IToken _token;
        public AuthController(IToken token)
        {
            _token = token;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            try
            {
                //if (user.Username.Trim() != request.Username.Trim())
                //{
                    _token.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    user.Username = request.Username;
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    
                //}
                //else
                //{
                //    return Ok($"User {request.Username} already registered");
                //}
            }
            catch (Exception ex)
            {

            }
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(UserDto request)
        {
            if (user.Username != request.Username)
            {
                return BadRequest("User not found.");
            }
            if (!_token.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }
            string token = _token.CreateToken(user);
            return Ok(token);
        }
    }
}
