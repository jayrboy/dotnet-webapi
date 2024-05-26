
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data;
using WebApi.Models;


namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private const string TokenSecret = "YourSecretKeyForAuthenticationOfApplicationDeveloper";

        private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(1);
        private EmployeeContext _db = new EmployeeContext();

        private readonly ILogger _logger;

        public AccountController(ILogger<AccountController> logger, IHostEnvironment environment)
        {
            _logger = logger;
        }

        [HttpPost("token")]
        public string GenerateToken([FromBody] string myToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(TokenSecret);
            var claims = new List<Claim>
            {
                // Add more claims as needed
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifetime),
                Issuer = "http://localhost:8000",
                Audience = "http://localhost:8000",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            if (token != null)
            {
                var jwt = tokenHandler.WriteToken(token);
                return jwt;
            }
            else
            {
                return "Token is required";
            }


        }

    }
}
