using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Data;
using WebApi.Models.Entities;
using WebApi.Models.DTOs;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class AuthController(MyDbContext db, IPasswordHasher<User> hasher, IConfiguration config) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // 1) check duplicate
        var exists = await db.Users.Where(u => u.Username == request.Username).FirstOrDefaultAsync();
        if (exists is not null)
        {
            return Conflict(new Response
            {
                Status_code = 409,
                Message = "Username already exists"
            });
        }

        // 2) create user
        var user = new User
        {
            Username = request.Username,
            Firstname = request.Firstname,
            Lastname = request.Lastname,
            Role = "USER",
            Status = "ACTIVE",
            CreatedAt = DateTime.Now,
            CreatedBy = "SYSTEM",
            UpdatedAt = DateTime.Now,
        };

        // 🔐 เก็บเป็น hash
        user.PasswordHash = hasher.HashPassword(user, request.Password);

        try
        {
            db.Users.Add(user);
            db.SaveChanges();
        }
        catch (DbUpdateException)
        {
            return BadRequest(new Response
            {
                Status_code = 400,
                Message = "Failed to create user",
            });
        }

        // 3) response: ห้ามส่ง password hash
        return StatusCode(201, new Response
        {
            Status_code = 201,
            Message = "Created Successfully",
            Data = new
            {
                user.UserId,
                user.Username,
                user.Role,
                user.Status,
                user.CreatedAt
            }
        });
    }

    [HttpPost("token")]
    public string GenerateToken([FromBody] User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(config["Jwt:Secret"]!);
        var claims = new List<Claim>
        {
            // Add more claims as needed
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Role, user.Role)

        };
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(config["Jwt:ExpiryMinutes"])
            ),
            Issuer = config["Jwt:Issuer"],
            Audience = config["Jwt:Audience"],
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        User? user = db.Users.Where(u => u.Username == request.Username).FirstOrDefault();

        if (user is null)
        {
            return NotFound(new Response
            {
                Status_code = 404,
                Message = "User not found"
            });
        }

        var result = hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
        {
            return BadRequest(new Response
            {
                Status_code = 400,
                Message = "Invalid password"
            });
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
                Status_code = 400,
                Message = "Bad Request to Username & Password",
            }
            );
        }
        return Ok(new Response
        {
            Status_code = 200,
            Message = "Login Success",
            Data = new
            {
                token = bearerToken,
            }
        });
    }

}