using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DTOs;

public struct LoginRequest
{

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}