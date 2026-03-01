using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models.Entities;
using WebApi.Models.DTOs;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private MyDbContext _db;

    private readonly IPasswordHasher<User> _hasher;

    public AuthController(MyDbContext db, IPasswordHasher<User> hasher)
    {
        _db = db;
        _hasher = hasher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        // 1) check duplicate
        var exists = await _db.Users.Where(u => u.Username == request.Username).FirstOrDefaultAsync();
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
        user.PasswordHash = _hasher.HashPassword(user, request.Password);

        try
        {
            _db.Users.Add(user);
            _db.SaveChanges();
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
}