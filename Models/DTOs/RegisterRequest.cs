using System.ComponentModel.DataAnnotations;

namespace WebApi.Models.DTOs;

public struct RegisterRequest
{

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    [Required]
    public string Firstname { get; set; }

    [Required]
    public string Lastname { get; set; }

    public string? Role { get; set; }
}