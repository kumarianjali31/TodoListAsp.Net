using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoListAPI.Model;

namespace TodoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterReqDto registerReqDto)
        {
            var identityUser = new IdentityUser()
            {
                UserName = registerReqDto.Username,
                Email = registerReqDto.Username
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerReqDto.Password);
            if (identityResult.Succeeded)
            {
                if (registerReqDto.Roles != null)
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerReqDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User created successfully");
                    }
                }
            }
            return BadRequest("not scucced");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginReqDto loginReqDto)
        {
            var identityUser = await userManager.FindByNameAsync(loginReqDto.Username);
            if (identityUser != null)
            {
                var checkPass = await userManager.CheckPasswordAsync(identityUser, loginReqDto.Password);
                if (checkPass)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);
                    if (roles != null)
                    {
                        var token = tokenRepository.CreateJwtToken(identityUser, roles.ToList());
                        var response = new LoginResponseDto()
                        {
                            JwtToken = token
                        };
                        return Ok(response);
                    }
                }
            }
            return BadRequest("not scucceed");
        }
    }
}
