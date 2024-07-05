
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data;
using WebApi.Models;
using WebApi.Dtos.Authorization;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private const string TokenSecret = "YourSecretKeyForAuthenticationOfApplicationDeveloper";

        private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(1);
        private EmployeeContext _db = new EmployeeContext();

        private readonly ILogger _logger;

        public AuthorizationController(ILogger<AuthorizationController> logger)
        {
            _logger = logger;
        }

        [HttpPost("token")]
        public string GenerateToken([FromBody] User user)
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

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Authorization/login
        ///     {
        ///         "username": "admin",
        ///         "password": "1234"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        /// <response code="200">Success</response>
        /// <response code="409">Conflict: Username already exists</response>
        [HttpPost("login", Name = "Login")]
        public IActionResult Login([FromBody] User request)
        {
            User? user = _db.Users.FirstOrDefault(doc => doc.Username == request.Username && doc.Password == request.Password && doc.IsDelete == false);

            if (user == null)
            {
                return NotFound();
            }

            string bearerToken;

            try
            {
                bearerToken = GenerateToken(user);
            }
            catch
            {
                return BadRequest(new Response
                {
                    Status = 400,
                    Message = "Bad Request to Username & Password",
                    Data = null
                }
                );
            }
            return Ok(new Response
            {
                Status = 200,
                Message = "Login Success",
                Data = new
                {
                    token = bearerToken,
                    role = user.Role
                }
            });
        }

        /// <summary>
        /// Register
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST api/Authorization/register
        ///     {
        ///         "username": "john",
        ///         "password": "1234"
        ///     }
        ///
        /// </remarks>
        /// <returns></returns>
        /// <response code="201">Created Successfully</response>
        /// <response code="409">Conflict: Username already exists</response>
        [HttpPost("register", Name = "Register")]
        public ActionResult Register(RegisterRequestDTO registerCreate)
        {
            if (registerCreate.Role.IsNullOrEmpty())
            {
                registerCreate.Role = "user";
            }

            User user = new User
            {
                Username = registerCreate.Username,
                Password = registerCreate.Password,
                Role = registerCreate.Role,
            };

            try
            {
                user = Models.User.Create(_db, user);
            }
            catch
            {
                // Handle unique key constraint violation (duplicate username)
                return StatusCode(409, new Response
                {
                    Status = 409,
                    Message = "Username already exists",
                    Data = null
                });
            }

            return user.Username == null || user.Password == null || user.Role == null
                ? BadRequest(new Response
                {
                    Status = 400,
                    Message = "Bad Request",
                    Data = null
                })
                : Ok(new Response
                {
                    Status = 201,
                    Message = "Created Successfully",
                    Data = user
                });
        }

    }
}
